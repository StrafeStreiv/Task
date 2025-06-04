namespace TaskManagementSys.Models
{
    public class AdminUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
    }
}
