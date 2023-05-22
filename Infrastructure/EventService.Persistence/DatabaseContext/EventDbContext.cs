using Application.Persistence.Configurations;
using EventService.Domain.Entities;
using EventService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EventService.Persistence.DatabaseContext
{
    public class EventDbContext : DbContext, IEventDbContext
    {
        public EventDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Event> Events => this.Set<Event>();

        public override int SaveChanges()
        {
            this.SetEntityTimeProperties();
            var result = base.SaveChanges();
            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            this.SetEntityTimeProperties();
            var result = await base.SaveChangesAsync(cancellationToken);
            return result;
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EventConfiguration());
            base.OnModelCreating(modelBuilder);
        }


        private void SetEntityTimeProperties()
        {
            this.SetCreationTime();
            this.SetUpdateTime();
        }


        private void SetUpdateTime()
        {
            var modifiedEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified)
            .Select(e => e.Entity);

            foreach (var entity in modifiedEntities)
            {
                PropertyInfo propertyInfo = entity.GetType().GetProperty("UpdatedAt")!;
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(entity, DateTime.UtcNow);
                }
            }
        }

        private void SetCreationTime()
        {
            var newEntities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added)
            .Select(e => e.Entity);

            foreach (var entity in newEntities)
            {
                PropertyInfo propertyInfo = entity.GetType().GetProperty("CreatedAt")!;
                if (propertyInfo != null)
                {
                    propertyInfo.SetValue(entity, DateTime.UtcNow);
                }
            }
        }
    }
}
