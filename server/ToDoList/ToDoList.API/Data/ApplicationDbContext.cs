using Microsoft.EntityFrameworkCore;
using ToDoList.API.Models;

namespace ToDoList.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed default user
            modelBuilder.Entity<User>().HasData(new User
            {
                UserId = Guid.NewGuid(),
                Username = "admin",
                Password = "password1"
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
