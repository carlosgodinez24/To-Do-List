using System.ComponentModel.DataAnnotations;

namespace ToDoList.API.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        // Navigation property
        public ICollection<TaskItem> Tasks { get; set; }
    }
}
