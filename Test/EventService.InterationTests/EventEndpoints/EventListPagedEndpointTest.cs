namespace EventService.IntegrationTests.EventEndpoints
{
    public class EventListPagedEndpointTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<Program> _factory;

        public EventListPagedEndpointTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
        }

        [Theory]
        [MemberData(nameof(ValidEventValidator))]
        public async Task ReturnsCorrectEventsGivenPageIndexAndPageSize(int pageIndex, int pageSize, Action<EventResponse> validator)
        {
            
            var response = await _client.GetAsync(EventScenariosBase.Get.GetPagedEventsUrl(pageIndex, pageSize));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<PaginatedResult<List<EventResponse>>>();

            Assert.False(model!.ErrorFlag);
            Assert.Equal(5, model.Response.Count());
            Assert.All(model.Response, validator);
        }


        [Fact]
        public async Task ReturnsEmptyEventsCollectionGivenOutOfRangePageIndex1()
        {         
            var response = await _client.GetAsync(EventScenariosBase.Get.GetPagedEventsUrl(3, 5));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<PaginatedResult<List<EventResponse>>>();

            Assert.False(model!.ErrorFlag);
            Assert.Empty(model.Response);
        }

        [Fact]
        public async Task ReturnsCorrectPageParamsGivenPageIndex1()
        {
            var response = await _client.GetAsync(EventScenariosBase.Get.GetPagedEventsUrl(1, 5));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var model = stringResponse.FromJson<PaginatedResult<List<EventResponse>>>();

            Assert.False(model!.ErrorFlag);
            Assert.Equal(5, model.Response.Count());
            Assert.Equal(10, model.PageParams.Total);
            Assert.Equal(1, model.PageParams.pageIndex);
            Assert.Equal(5, model.PageParams.pageSize);
        }



        public static IEnumerable<object[]> ValidEventValidator()
        {
            var firstFiveItems = new EventBuilder().BuildEventIds().Take(5).ToList();
            var nextFiveItems = new EventBuilder().BuildEventIds().Skip(5).ToList();

            var testData = new List<object[]>
            {
                new object[]
                {
                    1,
                    5,
                    new Action<EventResponse>(item =>
                    {
                        Assert.Contains<Guid>(item.EventId, firstFiveItems);
                    })
                },

                new object[]
                {
                    2,
                    5,
                    new Action<EventResponse>(item =>
                    {
                         Assert.Contains<Guid>(item.EventId, nextFiveItems);
                    })
                }

            };

            return testData;
        }
    }
}
