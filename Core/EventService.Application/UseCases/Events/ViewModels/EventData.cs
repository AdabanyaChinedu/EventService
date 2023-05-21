namespace EventService.Application.UseCases.Events.ViewModels
{
    public class EventData
    {
        public int UserId { get; set; }
        public string Title { get; set; } = default!;

        public string Description { get; set; } = default!;

        public string Location { get; set; } = default!;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TimeZone { get; set; } = default!;
    }
}
