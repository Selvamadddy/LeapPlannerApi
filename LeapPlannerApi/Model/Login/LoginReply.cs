using LeapPlannerApi.Model.Common;

namespace LeapPlannerApi.Model.Login
{
    public class LoginReply : ReplyBase
    {
        public JwtToken Token { get; set; }
    }
}
