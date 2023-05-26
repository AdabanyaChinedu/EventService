using EventService.Application.UseCases.Events.Commands;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Entities;
using EventService.Domain.Interfaces;
using EventService.UnitTests.Builders;
using Moq;

namespace EventService.UnitTests.Core.Application.MediatorHandlers
{
    public class EventCreateCommandHandlerTests : EventHandlerBase
    {

        private readonly Mock<IHttpService> _mockHttpService;
        private readonly Guid Id = new("2d930c2e-0fe2-4a60-9270-b8d94e42b155");

        public EventCreateCommandHandlerTests()
        {
            _mockHttpService = new Mock<IHttpService>();
            _mockHttpService.Setup(x => x.GetAsync<UserData>(It.IsAny<string>())).ReturnsAsync(new UserBuilder().BuildUser());
        }

        [Fact]
        public async Task EventCreateHandlerReturnsValidResponse()
        {
            var request = new EventCreate.Command(new EventData());

            var handler = new EventCreate.Handler(MockEventService.Object, MapperObj, _mockHttpService.Object);

            
            var @event = new Mock<Event>().Object;
            var eventUserResponse = new EventUserResponse();
            var userData = new UserData();

            MockEventService.Setup(m => m.AddEventAsync(It.IsAny<Event>()))
                .Callback<Event>(e => @event = e)
                .Returns(Task.FromResult<int>(1));

            _mockHttpService.Setup(m => m.GetAsync<UserData>(It.IsAny<string>()))
                .ReturnsAsync(userData);
            
            var result = await handler.Handle(request, CancellationToken.None);
            
            Assert.NotNull(result);
            MockEventService.Verify(m => m.AddEventAsync(@event), Times.Once);            
            _mockHttpService.Verify(m => m.GetAsync<UserData>(It.IsAny<string>()), Times.Once);
        }
    }
}
