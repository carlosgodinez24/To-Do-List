using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ToDoList.API.Data;
using ToDoList.API.Models;

namespace ToDoList.API.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Users;
        }

        public async Task<IEnumerable<User>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<User> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(User entity) => await _dbSet.AddAsync(entity);

        public void Update(User entity) => _dbSet.Update(entity);

        public void Delete(User entity) => _dbSet.Remove(entity);

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
