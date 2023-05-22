using EventService.Domain.Interfaces;

namespace EventService.IntegrationTests.Helpers
{
    public static class DatabaseHelper
    {
        public static void InitialiseDbForTests(IEventDbContext dbContext)
        {
            dbContext.Events.AddRange(new EventBuilder().BuildEvents());

            dbContext.SaveChanges();
        }

        public static void ResetDbForTests(IEventDbContext dbContext)
        {
            var events = dbContext.Events.ToArray();
            dbContext.Events.RemoveRange(events);

            dbContext.SaveChanges();

            InitialiseDbForTests(dbContext);
        }
    }
}
