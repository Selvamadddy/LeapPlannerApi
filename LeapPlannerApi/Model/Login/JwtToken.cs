namespace LeapPlannerApi.Model.Login
{
    public class JwtToken
    {
        public string Access_Token { get; set; }
        public string Scope { get; set; }
        public string ExpireIn { get; set; }
    }
}
