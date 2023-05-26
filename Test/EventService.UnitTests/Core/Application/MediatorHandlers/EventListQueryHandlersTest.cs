using AutoMapper;
using EventService.Application.UseCases.Events.Queries;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Entities;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Constants;
using EventService.SharedLibrary.Exceptions;
using EventService.SharedLibrary.Model.ResponseModel;
using EventService.UnitTests.Builders;
using Moq;
using System.Xml.Linq;

namespace EventService.UnitTests.Core.Application.MediatorHandlers
{
    public class EventListQueryHandlersTest : EventHandlerBase
    {

        private readonly Mock<IHttpService> _mockHttpService;
        private readonly Mock<IRedisService> _mockRedisService;

        public EventListQueryHandlersTest()
        {

            _mockHttpService = new Mock<IHttpService>();
            _mockHttpService.Setup(x => x.GetAsync<UserData>(It.IsAny<string>())).ReturnsAsync(new UserBuilder().BuildUser());
            _mockRedisService = new Mock<IRedisService>();
        }

        [Fact]
        public async Task EventListQueryHandlerReturnsCachedResultWhenCacheExists()
        {

            var request = new EventListQuery.Query(pageIndex: 1, pageSize: 10);
            var cacheKey = $"{EventConstants.AppName}:pageIndex-{request.pageIndex}:pagesize-{request.pageSize}";


            var cachedResult = new PaginatedResult<IEnumerable<EventResponse>>();
            _mockRedisService.Setup(m => m.GetAsync<PaginatedResult<IEnumerable<EventResponse>>>(cacheKey))
                .ReturnsAsync(cachedResult);

            var handler = new EventListQuery.QueryHandler(MockEventService.Object, MapperObj, _mockRedisService.Object);
            var result = await handler.Handle(request, CancellationToken.None);


            Assert.Same(cachedResult, result);
            _mockRedisService.Verify(m => m.GetAsync<PaginatedResult<IEnumerable<EventResponse>>>(cacheKey), Times.Once);
            _mockRedisService.Verify(m => m.SetAsync(cacheKey, It.IsAny<PaginatedResult<IEnumerable<EventResponse>>>(), 2), Times.Never);
            MockEventService.Verify(m => m.GetPagedEventAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }



        [Fact]
        public async Task EventListQueryHandlerReturnsResponseWhenCacheDoesNotExist()
        {
            var request = new EventListQuery.Query(pageIndex: 1, pageSize: 10);
            var cacheKey = $"{EventConstants.AppName}:pageIndex-{request.pageIndex}:pagesize-{request.pageSize}";

            var response = new PaginatedResult<IEnumerable<EventResponse>>();            
            var pageParams = new PageParams(request.pageIndex, request.pageSize, 10);
            response.Response = MapperObj.Map<IEnumerable<EventResponse>>(new List<Event>());
            response.PageParams = pageParams;

            _mockRedisService.Setup(m => m.GetAsync<PaginatedResult<IEnumerable<EventResponse>>>(cacheKey))
                .ReturnsAsync((string key) => null);


            MockEventService.Setup(m => m.GetPagedEventAsync(request.pageIndex, request.pageSize))
               .ReturnsAsync((new List<Event>(), pageParams));


            var handler = new EventListQuery.QueryHandler(MockEventService.Object, MapperObj, _mockRedisService.Object);


            var result = await handler.Handle(request, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal(response.Response, result.Response);
            Assert.Equal(response.Status, result.Status);
            _mockRedisService.Verify(m => m.GetAsync<PaginatedResult<IEnumerable<EventResponse>>>(cacheKey), Times.Once);
            _mockRedisService.Verify(m => m.SetAsync(cacheKey, It.IsAny<PaginatedResult<IEnumerable<EventResponse>>>(), 2), Times.Once);
            MockEventService.Verify(m => m.GetPagedEventAsync(request.pageIndex, request.pageSize), Times.Once);
        }
    }
}
