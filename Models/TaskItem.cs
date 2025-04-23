using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagementSys.Models;

namespace TaskManagementSys.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название задачи обязательно")]
        [Display(Name = "Название задачи")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Required]
        [Display(Name = "Статус")]
        public TaskStatus Status { get; set; } = TaskStatus.Новая;

        [Display(Name = "Дата завершения")]
        public DateTime? DueDate { get; set; }

        [Display(Name = "Приоритет")]
        public TaskPriority Priority { get; set; } = TaskPriority.Обычный;

        [Display(Name = "Пользователь")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
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
