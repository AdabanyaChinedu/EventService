namespace EventService.Application.UseCases.Events.ViewModels
{
    public class EventUserResponse : EventResponse
    {
        public UserData user { get; set; } = default!;
    }
}
