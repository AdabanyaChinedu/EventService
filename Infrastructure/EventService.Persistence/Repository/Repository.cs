using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventService.Persistence.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        private readonly IEventDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(IEventDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<TEntity>();
        }

        public async Task<int> AddAsync(TEntity entity)
        {
           await _dbSet.AddAsync(entity);
           return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<(IEnumerable<TEntity>, PageParams)> GetPagedAsync(int pageIndex , int pageSize)
        {
            var query = _dbSet.AsQueryable();
            var totalItems = await query.CountAsync();

            var result = await query.Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return (result, new PageParams(pageIndex, pageSize, totalItems));
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<int> RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
