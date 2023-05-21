
using EventService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EventService.Domain.Interfaces
{
    public interface IEventDbContext
    {
        DbSet<Event> Events { get; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        /// <summary>
        /// Gets database object.
        /// </summary>
        public DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        int SaveChanges();
    }
}
