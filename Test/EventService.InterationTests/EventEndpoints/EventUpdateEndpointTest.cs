using EventService.IntegrationTests.Helpers;
using System.Text;

namespace EventService.IntegrationTests.EventEndpoints
{
    public class EventUpdateEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
    { 
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b543");//Invalid Event Id
        private readonly string Title = "Event 12";
        private readonly string Description = "Description 12";
        private readonly string Location = "Location 12";
        private readonly DateTime StartDate = FixedDateTime.UtcNow.AddHours(1);
        private readonly DateTime EndDate = FixedDateTime.UtcNow.AddHours(4);
        private readonly string TimeZone = "Africa/Lagos";
        private readonly int UserId = 6;

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EventUpdateEndpointTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task CreateEventReturns400GivenInvalidUserId()
        {
            var response = await _client.GetAsync(EventScenariosBase.Get.GetEventByIdUrl(Id));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateEventReturnsSuccessGivenValidParameters()
        {
            var id = new EventBuilder().BuildEventIds().First();
            var requestBody =  new EventData
                {
                    Title = Title,
                    Description = Description,
                    Location = Location,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TimeZone = TimeZone,
                    UserId = UserId,
                }.ToJson<EventData>();

            var payload = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync(EventScenariosBase.Put.UpdateEventUrl(id), payload);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<Result<EventResponse>>();

            Assert.False(model!.ErrorFlag);
            Assert.Equal(UserId, model.Response.UserId);
            Assert.Equal(Title, model.Response.Title);
            Assert.Equal(Description, model.Response.Description);
            Assert.Equal(Location, model.Response.Location);
            Assert.Equal(TimeZone, model.Response.TimeZone);
            Assert.Equal(StartDate, model.Response.StartDate);
            Assert.Equal(EndDate, model.Response.EndDate);

            var response2 = await _client.GetAsync(EventScenariosBase.Get.GetEventByIdUrl(id));
            response2.EnsureSuccessStatusCode();
            var stringResponse2 = await response2.Content.ReadAsStringAsync();
            var model2 = stringResponse2.FromJson<Result<EventResponse>>();

            Assert.False(model2!.ErrorFlag);
            Assert.Equal(UserId, model2.Response.UserId);
            Assert.Equal(Title, model2.Response.Title);
            Assert.Equal(Description, model2.Response.Description);
            Assert.Equal(Location, model2.Response.Location);
            Assert.Equal(TimeZone, model2.Response.TimeZone);
            Assert.Equal(StartDate, model2.Response.StartDate);
            Assert.Equal(EndDate, model2.Response.EndDate);
        }
    }
}
