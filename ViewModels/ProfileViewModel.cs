using System.ComponentModel.DataAnnotations;

namespace TaskManagementSys.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "ФИО")]
        public string FullName { get; set; }

        [Display(Name = "Команда")]
        public string TeamName { get; set; }

        [Display(Name = "Тема оформления")]
        public string Theme { get; set; } = "light"; // light или dark

        public List<string>? TeamMembers { get; set; }
    }
}
