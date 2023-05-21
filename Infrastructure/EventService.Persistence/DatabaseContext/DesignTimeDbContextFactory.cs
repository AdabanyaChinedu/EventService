using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace EventService.Persistence.DatabaseContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EventDbContext>
    {
        /// <summary>
        /// gets the TenantTeamMember property.
        /// </summary>
        /// <returns>TenantServiceDBContext.</returns>
        /// <param name="args">args.</param>
        public EventDbContext CreateDbContext(string[] args)
        {
            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("secrets.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddJsonFile($"appsettings.{envName}.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true)
                .Build();

            var builder = new DbContextOptionsBuilder<EventDbContext>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlite(connectionString!);
            return new EventDbContext(builder.Options);
        }
    }
}
