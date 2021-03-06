﻿using System;
using System.ComponentModel.DataAnnotations;
using DAL.Entities;

namespace BLL.Models
{
    public class AssignmentDto
    {
        public Guid? Id { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Name length cannot be more than 100 characters.")]
        public string Name { get; set; }
        [StringLength(1000, ErrorMessage = "Description length cannot be more than 1000 characters.")]
        public string Description { get; set; }
        [Range(0, 4)]
        public int? Priority { get; set; }
        [Range(0, 100)]
        public int? CompletionPercent { get; set; }
        [Required]
        public AssignmentStatus Status { get; set; }
        [Deadline]
        public DateTime? Deadline { get; set; }
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid AssigneeId { get; set; }
    }
}