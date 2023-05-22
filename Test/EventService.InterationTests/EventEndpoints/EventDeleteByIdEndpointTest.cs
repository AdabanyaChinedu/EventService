namespace EventService.IntegrationTests.EventEndpoints
{
    public class EventDeleteByIdEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b543");//Invalid Event Id
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EventDeleteByIdEndpointTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Fact]
        public async Task DeleteByIdReturnsNotFoundGivenInvalidId()
        {
            var response = await _client.GetAsync(EventScenariosBase.Delete.RemoveEventUrl(Id));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteEventRemovesEventGivenValidId()
        {
            var id = new EventBuilder().BuildEventIds().First();
            var response = await _client.DeleteAsync(EventScenariosBase.Delete.RemoveEventUrl(id));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<Result<string>>();
            Assert.False(model!.ErrorFlag);
            Assert.Equal("Delete successful", model.Response);


            var response2 = await _client.GetAsync(EventScenariosBase.Get.GetEventByIdUrl(id));
            Assert.Equal(HttpStatusCode.NotFound, response2.StatusCode);

        }
    }
}
