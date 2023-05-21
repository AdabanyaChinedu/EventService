using AutoMapper;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Entities;

namespace EventService.Application.AutoMapperConfig
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {

            this.CreateMap<EventData, Event>().ReverseMap();
            this.CreateMap<EventUserResponse, Event>()
                .ReverseMap()
                .ForMember(x => x.EventId,
                opt => opt.MapFrom(x => x.Id));
            this.CreateMap<EventResponse, Event>()
                .ReverseMap()
                .ForMember(x => x.EventId,
                opt => opt.MapFrom(x => x.Id));
        }
    }
}
