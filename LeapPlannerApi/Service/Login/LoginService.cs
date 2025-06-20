using LeapPlannerApi.Entities.Login;
using LeapPlannerApi.Repository.Login;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Login
{
    public class LoginService : ILoginService
    {
        private ILoginRepo _loginRepo;
        private IConfiguration _configuration;

        public LoginService(ILoginRepo loginRepo, IConfiguration configuration)
        {
            _configuration = configuration;
            _loginRepo = loginRepo;
        }

        public async Task<bool> AddUser(RegisterUserDto registerUser)
        {
            bool response = false;
            try
            {
                response = await _loginRepo.AddUser(new RegisterUserDto() { 
                    Email = registerUser.Email,
                    Name = registerUser.Name,
                    Password = HashPassword(registerUser.Password)
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<string> LoginUser(LoginDto loginDto)
        {
            string response = null;
            try
            {
                var user = await _loginRepo.GetUserDetails(loginDto.Email);
                if(user != null && !user.IsLocked)
                {
                    bool isValidPassword = VerifyPassword(user.HashedPassword, loginDto.Password);
                    if (isValidPassword)
                    {
                        response = GenerateToken(loginDto);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> UpdateUserPassword(LoginDto loginDto)
        {
            bool response = false;
            try
            {
                var user = await GetUserDetails(loginDto.Email);
                if (user != null)
                {
                    var otpDetail = await _loginRepo.GetUserOtp(user.Id);
                    if (otpDetail != null && otpDetail.CanUpdatePassword)
                    {
                        response = await _loginRepo.UpdateUserPassword(loginDto.Email, HashPassword(loginDto.Password));
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<UserDetails> GetUserDetails(string email)
        {
            UserDetails userDetails = new UserDetails();
            try
            {
                userDetails = await _loginRepo.GetUserDetails(email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userDetails;
        }

        public async Task<bool> GenerateOtp(string email)
        {
            bool response = false;
            try
            {
                var userDetails = await _loginRepo.GetUserDetails(email);
                if(userDetails != null)
                {
                    Random rnd = new Random();
                    int otp = rnd.Next(111111, 999999);
                   // var hashedOtp = HashPassword(otp.ToString());
                    var hashedOtp = HashPassword("123456");
                    response = await _loginRepo.AddOtp(new OtpDetail() { UserId = userDetails.Id, Otp = hashedOtp, CreatedDate = DateTime.Now.ToString() });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> ValidateOtp(string email, string otp)
        {
            bool response = false;
            try
            {
                var userDetails = await _loginRepo.GetUserDetails(email);
                if (userDetails != null)
                {
                    var otpDetail = await _loginRepo.GetUserOtp(userDetails.Id);
                    if(otpDetail != null && !otpDetail.CanUpdatePassword)
                    {
                        DateTime otpCreatedDate;
                        if (DateTime.TryParse(otpDetail.CreatedDate, out otpCreatedDate))
                        {
                            response = (DateTime.Compare(otpCreatedDate.AddMinutes(5), DateTime.Now) > 0) && VerifyPassword(otpDetail.Otp, otp);
                            if (response)
                            {
                                await _loginRepo.UpdateOtpStatus(otpDetail);
                            }
                        }                
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }




        private string GenerateToken(LoginDto loginDto)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                new Claim("Email",loginDto.Email)
                };

                var token = new JwtSecurityToken(_configuration.GetValue<string>("Jwt:Issuer"), null,
                    claims,
                    expires: DateTime.Now.AddMinutes(_configuration.GetValue<double>("Jwt:LifeSpan")),
                    signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string HashPassword(string password)
        {
            // 1. Generate a salt
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            // 2. Derive a key (hash)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        private bool VerifyPassword(string hashedPasswordWithSalt, string password)
        {
            try
            {
                var parts = hashedPasswordWithSalt.Split('.');
                if (parts.Length != 2)
                {
                    return false;
                }
                string saltString = parts[0];
                string savedHash = parts[1];

                byte[] salt = Convert.FromBase64String(saltString);

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                return hashed == savedHash;
            }
            catch
            {
                return false;
            }
        }

        private string CreateOtp()
        {
            Random rnd = new Random();
            int otp = rnd.Next(111111, 999999);
            return HashPassword(otp.ToString());
        }
    }
}
