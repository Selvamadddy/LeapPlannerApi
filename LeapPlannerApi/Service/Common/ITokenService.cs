using LeapPlannerApi.Entities.Login;
using Microsoft.AspNetCore.Http;

namespace LeapPlannerApi.Service.Common
{
    public interface ITokenService
    {
        string GenerateToken(LoginDto loginDto);
        (bool, string) ValidateToken(IHeaderDictionary headerDictionary);
    }
}
