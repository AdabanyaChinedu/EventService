using EventService.Domain.Interfaces;
using EventService.Infrastructure.HttpHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace EventService.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddTransient<IHttpService, HttpService>();
            return services;
        }
    }
}
