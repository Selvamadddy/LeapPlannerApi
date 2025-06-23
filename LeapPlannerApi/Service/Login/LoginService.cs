using LeapPlannerApi.Entities.Login;
using LeapPlannerApi.Repository.Login;
using LeapPlannerApi.Service.Common;
using LeapPlannerApi.Service.Planner;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Login
{
    public class LoginService : ILoginService
    {
        private ILoginRepo _loginRepo;
        private ITokenService _tokenService;
        private IPlannerService _plannerService;

        public LoginService(ILoginRepo loginRepo, ITokenService tokenService, IPlannerService plannerService)
        {
            _loginRepo = loginRepo;
            _tokenService = tokenService;
            _plannerService = plannerService;
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
                if (response)
                {
                    await _plannerService.AddPlanner(registerUser.Email, "");
                }
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
                        response =  _tokenService.GenerateToken(loginDto);
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
    }
}
