using EventService.Application.UseCases.Events.Queries;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Exceptions;
using EventService.SharedLibrary.Model.ResponseModel;
using EventService.UnitTests.Builders;
using Moq;

namespace EventService.UnitTests.Core.Application.MediatorHandlers
{
    public class EventDetailsQueryHandlerTests: EventHandlerBase
    {
        
        private readonly Mock<IHttpService> _mockHttpService;
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b155");

        public EventDetailsQueryHandlerTests()
        {           
                     
            _mockHttpService = new Mock<IHttpService>();
            _mockHttpService.Setup(x => x.GetAsync<UserData>(It.IsAny<string>())).ReturnsAsync(new UserBuilder().BuildUser());
        }

        [Fact]
        public async Task EventDetailQueryHandlerReturnsValidResponseWhenEventExists()
        {            
            var @event = new EventBuilder().BuildEvents().First(x => x.Id == Id);
            MockEventService.Setup(x => x.GetByEventIdAsync(It.IsAny<Guid>())).ReturnsAsync(@event);

            var request = new EventDetail.Query(Id);

            var handler = new EventDetail.QueryHandler(MockEventService.Object, MapperObj, _mockHttpService.Object);

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.IsType<Result<EventUserResponse>>(result);
            Assert.Equal(Id, result.Response.EventId);
            Assert.Equal(1, result.Response.user.Id);
            MockEventService.Verify(m => m.GetByEventIdAsync(request.Id), Times.Once);
            _mockHttpService.Verify(m => m.GetAsync<UserData>(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task EventDetailQueryHandlerThrowsEntityNotFoundExceptionWhenEventDoesNotExists()
        {
            MockEventService.Setup(x => x.GetByEventIdAsync(It.IsAny<Guid>())).ReturnsAsync((Guid id) => null);
            var request = new EventDetail.Query(Id);

            var handler = new EventDetail.QueryHandler(MockEventService.Object, MapperObj, _mockHttpService.Object);

            await Assert.ThrowsAsync<EntityNotFoundException>(async () => await handler.Handle(request, CancellationToken.None));
            MockEventService.Verify(m => m.GetByEventIdAsync(request.Id), Times.Once);
            _mockHttpService.Verify(m => m.GetAsync<UserData>(It.IsAny<string>()), Times.Never);
        }
    }
}
