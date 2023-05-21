using AutoMapper;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Exceptions;
using EventService.SharedLibrary.Model.ResponseModel;
using FluentValidation;
using MediatR;

namespace EventService.Application.UseCases.Events.Queries
{

    public class EventDetail
    {
        public record Query(Guid Id) : IRequest<Result<EventUserResponse>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Id)
                     .NotEmpty()
                     .WithMessage("Id is required.")
                     .NotEqual(Guid.Empty)
                     .WithMessage("Invalid Guid.");
            }
        }

        public class QueryHandler : IRequestHandler<Query, Result<EventUserResponse>>
        {
            private readonly IEventService _eventService;
            private readonly IMapper _mapper;
            private readonly IHttpService _httpService;

            public QueryHandler(IEventService eventService, IMapper mapper, IHttpService httpService)
            {
                _eventService = eventService;
                _mapper = mapper;
                _httpService = httpService;
            }

            public async Task<Result<EventUserResponse>> Handle(Query request, CancellationToken cancellationToken)
            {
                var response = new Result<EventUserResponse>();

                var result = await _eventService.GetByEventIdAsync(request.Id);

                if (result == null)
                {
                    throw new EntityNotFoundException($"Event with {request.Id} does not exist");
                }

                 var eventRsp = _mapper.Map<EventUserResponse>(result);

                eventRsp.user = await _httpService.GetAsync<UserData>($"https://jsonplaceholder.typicode.com/users/{result.UserId}");

                response.Response = eventRsp;

                return response;
            }
        }
    }
}
