using Dapper;
using LeapPlannerApi.Entities.Login;
using System;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Login
{
    public class LoginRepo : ILoginRepo
    {
        private readonly DapperContext _context;
        public LoginRepo(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUser(RegisterUserDto registerUser)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("AddUser", registerUser, commandType: System.Data.CommandType.StoredProcedure);
                    response = result > 0;
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
            UserDetails response = new UserDetails();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.QuerySingleOrDefaultAsync<UserDetails>("GetUser", new { email = email }, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> UpdateUserPassword(string email, string updatedPassword)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("UpdateUserPassword", new { email = email, updatedHashedPassword = updatedPassword }, commandType: System.Data.CommandType.StoredProcedure);
                    response = result > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> AddOtp(OtpDetail otpDetail)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("AddOtp", new { otp = otpDetail.Otp, userId = otpDetail.UserId, createdDate = otpDetail.CreatedDate }, commandType: System.Data.CommandType.StoredProcedure);
                    response = result > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<OtpDetail> GetUserOtp(int userId)
        {
            OtpDetail response = new OtpDetail();
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    response = await connection.QuerySingleOrDefaultAsync<OtpDetail>("GetOtp", new { userId = userId }, commandType: System.Data.CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<bool> UpdateOtpStatus(OtpDetail otpDetail)
        {
            bool response = false;
            try
            {
                using (var connection = _context.CreateConnection())
                {
                    var result = await connection.ExecuteAsync("UpdateOtp", new {otpId = otpDetail.Id}, commandType: System.Data.CommandType.StoredProcedure);
                    response = result > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }
    }
}
