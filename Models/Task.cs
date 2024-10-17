using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Description { get; set; }
        [Required]
        public TaskStatus Status { get; set; } = Models.Task.TaskStatus.Todo;
        [Display(Name = "Created At")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime CreatedAt { get; set; }
        [Display(Name = "Updated At")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        public enum TaskStatus
        {
            Todo,
            InProgress,
            Done
        }
    }
}