
namespace LeapPlannerApi.Entities.Login
{
    public class UserDetails
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public bool IsLocked { get; set; }
    }
}
