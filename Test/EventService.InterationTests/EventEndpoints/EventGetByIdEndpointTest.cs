namespace EventService.IntegrationTests.EventEndpoints
{
    public class EventGetByIdEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b543");//Invalid Event Id
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EventGetByIdEndpointTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task GetByIdReturnsNotFoundGivenInvalidId()
        {
            var response = await _client.GetAsync(EventScenariosBase.Get.GetEventByIdUrl(Id));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetByIdReturnsItemGivenValidId()
        {
            var id = new EventBuilder().BuildEventIds().First();
            var response = await _client.GetAsync(EventScenariosBase.Get.GetEventByIdUrl(id));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<Result<EventResponse>>();

            Assert.False(model!.ErrorFlag);
            Assert.Equal(id, model.Response.EventId);
        }

    }
}
