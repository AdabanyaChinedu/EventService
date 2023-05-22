using EventService.Domain.Interfaces;
using EventService.IntegrationTests.Helpers;
using EventService.Persistence.DatabaseContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EventService.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        private readonly InMemoryDatabaseRoot _dbRoot = new InMemoryDatabaseRoot();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureTestServices(services =>
            {
                var descriptor = services.SingleOrDefault<ServiceDescriptor>(
                    d => d.ServiceType ==
                        typeof(DbContextOptions<EventDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<EventDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting", _dbRoot);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<IEventDbContext>();                    

                    db.Database.EnsureCreated();

                    try
                    {
                        DatabaseHelper.InitialiseDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                    }
                }
            });
        }
    }
}