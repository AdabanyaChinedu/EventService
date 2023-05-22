using EventService.IntegrationTests.Extensions;
using EventService.IntegrationTests.Helpers;
using System.Text;

namespace EventService.IntegrationTests.EventEndpoints
{
    public class EventCreateEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b543");//Invalid Event Id
        private readonly string Title = "Event 11";
        private readonly string Description = "Description 11";
        private readonly string Location = "Location 11";
        private readonly DateTime StartDate = FixedDateTime.UtcNow;
        private readonly DateTime EndDate = FixedDateTime.UtcNow.AddHours(3);
        private readonly string TimeZone = "Australia/Sydney";
        private readonly int UserId = 5;

        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EventCreateEndpointTest(CustomWebApplicationFactory<Program> factory)
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
        public async Task CreateEventReturnsSuccessGivenValidParameters()
        {
            var requestBody = new {
                Event = new EventData
                {
                    Title = Title,
                    Description = Description,
                    Location = Location,
                    StartDate = StartDate,
                    EndDate = EndDate,
                    TimeZone = TimeZone,
                    UserId = UserId,
                },
            }.ToJson<object>();

            var payload = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(EventScenariosBase.Post.CreateEventUrl, payload);
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
        }
    }
}
