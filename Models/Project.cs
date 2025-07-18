﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskManagementSys.Models;

namespace TaskManagementSys.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
