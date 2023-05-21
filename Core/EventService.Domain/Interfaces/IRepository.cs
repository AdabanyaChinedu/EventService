using EventService.SharedLibrary.Model.ResponseModel;
using System.Linq.Expressions;

namespace EventService.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IAggregateRoot
    {
        Task<int> AddAsync(TEntity entity);

        Task<int> UpdateAsync(TEntity entity);

        Task<IEnumerable<TEntity>> GetBy(Expression<Func<TEntity, bool>> predicate);

        Task<(IEnumerable<TEntity>, PageParams)> GetPagedAsync(int pageIndex, int pageSize);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);

        Task<int> RemoveAsync(TEntity entity);
    }
}
