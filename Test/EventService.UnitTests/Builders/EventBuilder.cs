using EventService.Domain.Entities;

namespace EventService.UnitTests.Builders
{
    public class EventBuilder
    {
        private readonly List<Event> _events;

        public EventBuilder()
        {
            _events = DefaultEvents();
        }

        public List<Event> BuildEvents()
        {
            return _events;
        }

        public List<Event> DefaultEvents()
        {
            List<Event> events = new List<Event>
            {
                new Event(new("2d930c2e-0fe2-4a60-9270-b8d94e42b155"), "Event 1", "Description 1", "Location 1", DateTime.Now, DateTime.Now.AddHours(1), "Pacific Standard Time", 1),
                new Event(new("8e81330b-9d1f-42d7-9f3b-c1dfc24d4324"), "Event 2", "Description 2", "Location 2", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(2), "America/New_York", 2),
                new Event(new("d1cde268-83c3-42d1-88c1-1500cb96380b"), "Event 3", "Description 3", "Location 3", DateTime.Now.AddDays(2), DateTime.Now.AddDays(2).AddHours(3), "Europe/London", 3),
                new Event(new("0a1f4800-3944-4cbe-90f7-7be37ed0ab75"), "Event 4", "Description 4", "Location 4", DateTime.Now.AddDays(3), DateTime.Now.AddDays(3).AddHours(4), "Asia/Tokyo", 4),
                new Event(new("ef2a2eaf-62c2-47f9-9e6b-d7d348e232b9"), "Event 5", "Description 5", "Location 5", DateTime.Now.AddDays(4), DateTime.Now.AddDays(4).AddHours(5), "Australia/Sydney", 5),
                new Event(new("6758a3e4-0a7a-4b0f-a2f7-22ff877eeb75"), "Event 6", "Description 6", "Location 6", DateTime.Now.AddDays(5), DateTime.Now.AddDays(5).AddHours(2), "America/New_York", 6),
                new Event(new("ee6d9d3a-4744-4c89-829e-f60b8f7e7915"), "Event 7", "Description 7", "Location 7", DateTime.Now.AddDays(6), DateTime.Now.AddDays(6).AddHours(3), "Europe/London", 7),
                new Event(new("4c6c1c9f-4b99-48a9-9b56-3b2341dd413a"), "Event 8", "Description 8", "Location 8", DateTime.Now.AddDays(6), DateTime.Now.AddDays(6).AddHours(4), "Pacific Standard Time", 1),
                new Event(new("4a2ac1ad-a2a6-4bf5-87e8-709558830c74"), "Event 9", "Description 9", "Location 9", DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddHours(5), "Eastern Standard Time", 2),
                new Event(new("25072824-a09b-45cf-b468-8b98c5fdac07"), "Event 10", "Description 10", "Location 10", DateTime.Now.AddDays(2), DateTime.Now.AddDays(2).AddHours(2), "America/New_York", 3),
            };
            return events;
        }
    }
}
