using Microsoft.AspNetCore.Identity;
namespace TaskManagementSys.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string? FullName { get; set; }
    }
}
