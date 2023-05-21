namespace EventService.Application.UseCases.Events.ViewModels
{
    public class EventResponse
    {
        public Guid EventId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public string Location { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TimeZone { get; set; } = default!;

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }
    }
}
