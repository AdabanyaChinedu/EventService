using EventService.Domain.Interfaces;
using EventService.Persistence.DatabaseContext;
using EventService.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventService.Persistence.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddDbContext<EventDbContext>(options =>
                            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")!));
            services.AddScoped<IEventDbContext>(provider => provider.GetRequiredService<EventDbContext>());
            return services;
        }
    }
}
