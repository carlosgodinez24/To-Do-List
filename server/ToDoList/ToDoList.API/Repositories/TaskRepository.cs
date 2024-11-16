using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoList.API.Data;
using ToDoList.API.Models;

namespace ToDoList.API.Repositories
{
    public class TaskRepository : IRepository<TaskItem>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TaskItem> _dbSet;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Tasks;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<TaskItem> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<TaskItem>> FindAsync(Expression<Func<TaskItem, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(TaskItem entity) => await _dbSet.AddAsync(entity);

        public void Update(TaskItem entity) => _dbSet.Update(entity);

        public void Delete(TaskItem entity) => _dbSet.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
