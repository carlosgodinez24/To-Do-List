using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    public class TaskItem
    {
        [Key]
        public Guid TaskId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }

        // Foreign Key
        [Required]
        public Guid UserId { get; set; }

        // Navigation property
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
