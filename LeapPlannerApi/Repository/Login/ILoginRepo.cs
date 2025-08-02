using LeapPlannerApi.Entities.Login;
using System.Threading.Tasks;

namespace LeapPlannerApi.Repository.Login
{
    public interface ILoginRepo
    {
        Task<bool> AddUser(RegisterUserDto registerUser);
        Task<UserDetails> GetUserDetails(string email);
        Task<bool> UpdateUserPassword(string email, string updatedPassword);
        Task<OtpDetail> GetUserOtp(int userId);
        Task<bool> AddOtp(OtpDetail otpDetail);
        Task<bool> UpdateOtpStatus(OtpDetail otpDetail);
    }
}
