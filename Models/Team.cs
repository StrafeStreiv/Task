using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskManagementSys.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название команды")]
        public string Name { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
