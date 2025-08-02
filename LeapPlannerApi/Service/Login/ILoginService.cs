using LeapPlannerApi.Entities.Login;
using System.Threading.Tasks;

namespace LeapPlannerApi.Service.Login
{
    public interface ILoginService
    {
        Task<bool> AddUser(RegisterUserDto registerUser);
        Task<string> LoginUser(LoginDto loginDto);
        Task<UserDetails> GetUserDetails(string email);
        Task<bool> GenerateOtp(string email);
        Task<bool> ValidateOtp(string email, string otp);
        Task<bool> UpdateUserPassword(LoginDto loginDto);
    }
}
