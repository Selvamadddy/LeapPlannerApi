namespace LeapPlannerApi.Entities.Login
{
    public class OtpDetail
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Otp { get; set; }
        public string CreatedDate { get; set; }
        public bool CanUpdatePassword { get; set; }
    }
}