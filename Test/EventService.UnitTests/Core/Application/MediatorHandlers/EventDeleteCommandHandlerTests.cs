using EventService.Application.UseCases.Events.Commands;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Entities;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Exceptions;
using EventService.UnitTests.Builders;
using Moq;

namespace EventService.UnitTests.Core.Application.MediatorHandlers
{
    public class EventDeleteCommandHandlerTests : EventHandlerBase
    {
        private readonly Mock<IHttpService> _mockHttpService;
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b155");

        public EventDeleteCommandHandlerTests()
        {

            _mockHttpService = new Mock<IHttpService>();
            _mockHttpService.Setup(x => x.GetAsync<UserData>(It.IsAny<string>())).ReturnsAsync(new UserBuilder().BuildUser());
        }

        [Fact]
        public async Task DeleteEventHandlerReturnsSuccessfulResponseWhenEventExists()
        {
            
            var request = new EventDelete.Command(Id);

            var handler = new EventDelete.Handler(MockEventService.Object);


            var @event = new Mock<Event>().Object;
            MockEventService.Setup(m => m.GetByEventIdAsync(request.Id))
                .ReturnsAsync(@event);

            MockEventService.Setup(m => m.RemoveEventAsync(@event))
                .Returns(Task.FromResult(1));

            
            var result = await handler.Handle(request, CancellationToken.None);

            
            Assert.NotNull(result);
            Assert.Equal("Delete successful", result.Response);
            MockEventService.Verify(m => m.GetByEventIdAsync(request.Id), Times.Once);
            MockEventService.Verify(m => m.RemoveEventAsync(@event), Times.Once);
        }

        [Fact]
        public async Task DeleteEventHandlerThrowsEntityNotFoundExceptionWhenEventDoesNotExists()
        {
            var request = new EventDelete.Command(Id);

            var handler = new EventDelete.Handler(MockEventService.Object);

            MockEventService.Setup(m => m.GetByEventIdAsync(request.Id))
                .ReturnsAsync(() => null); 
            
            await Assert.ThrowsAsync<EntityNotFoundException>(async () =>  await handler.Handle(request, CancellationToken.None));
            MockEventService.Verify(m => m.GetByEventIdAsync(request.Id), Times.Once);
            MockEventService.Verify(m => m.RemoveEventAsync(It.IsAny<Event>()), Times.Never);
        }
    }
}