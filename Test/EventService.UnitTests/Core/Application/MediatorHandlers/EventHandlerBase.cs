using AutoMapper;
using EventService.Application.AutoMapperConfig;
using EventService.Domain.Interfaces;
using Moq;

namespace EventService.UnitTests.Core.Application.MediatorHandlers
{
    public class EventHandlerBase
    {
        public IMapper MapperObj { get; }
        public Mock<IEventService> MockEventService { get; }
        public EventHandlerBase()
        {
            MockEventService = new Mock<IEventService>();
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            MapperObj = new Mapper(mapperConfiguration);
        }
    }
}
