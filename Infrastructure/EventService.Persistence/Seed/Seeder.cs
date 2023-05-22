using EventService.Domain.Interfaces;

namespace EventService.Persistence.Seed
{
    public static class Seeder
    {
        public static async Task SeedDataAsync(IEventDbContext context)
        {
            await SeedSampleEvents(context);
        }

        private static async Task SeedSampleEvents(IEventDbContext context)
        {          

            
        }
    }
}
