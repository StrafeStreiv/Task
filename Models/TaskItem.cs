using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagementSys.Models;

namespace TaskManagementSys.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Display(Name = "Статус")]
        public TaskStatus Status { get; set; }

        [Display(Name = "Приоритет")]
        public TaskPriority Priority { get; set; }

        [Display(Name = "Срок")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }

    public enum TaskStatus
    {
        Новая,
        В_работе,
        Завершена
    }

    public enum TaskPriority
    {
        Низкий,
        Обычный,
        Высокий
    }
}
