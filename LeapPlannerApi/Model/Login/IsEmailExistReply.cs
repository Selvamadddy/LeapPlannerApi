using LeapPlannerApi.Model.Common;

namespace LeapPlannerApi.Model.Login
{
    public class IsEmailExistReply : ReplyBase
    {
        public bool IsUserExist { get; set; }
    }
}
