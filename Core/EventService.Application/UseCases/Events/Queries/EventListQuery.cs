using AutoMapper;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Constants;
using EventService.SharedLibrary.Model.ResponseModel;
using FluentValidation;
using MediatR;

namespace EventService.Application.UseCases.Events.Queries
{
    public class EventListQuery
    {
        public record Query(int pageIndex, int pageSize) : IRequest<PaginatedResult<IEnumerable<EventResponse>>>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.pageIndex)
                     .NotEmpty()
                     .WithMessage("{PropertyName} is required.")
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} must be greater than 0.");

                RuleFor(x => x.pageSize)
                     .NotEmpty()
                     .WithMessage("{PropertyName} is required.")
                    .GreaterThan(0)
                    .WithMessage("{PropertyName} must be greater than 0.");
            }
        }
        public class QueryHandler : IRequestHandler<Query, PaginatedResult<IEnumerable<EventResponse>>>
        {
            private readonly IEventService _eventService;
            private readonly IMapper _mapper;
            private readonly IRedisService _redisService;

            public QueryHandler(IEventService eventService, IMapper mapper, IRedisService redisService)
            {
                _eventService = eventService;
                _mapper = mapper;
                _redisService = redisService;

            }

            public async Task<PaginatedResult<IEnumerable<EventResponse>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var cacheKey = $"{EventConstants.AppName}:pageIndex-{request.pageIndex}:pagesize-{request.pageSize}";
                var cacheResult = await _redisService.GetAsync<PaginatedResult<IEnumerable<EventResponse>>>(cacheKey);

                if (cacheResult != null)
                {
                    return cacheResult;
                }
                var response = new PaginatedResult<IEnumerable<EventResponse>>();

                var (result, pageparams) = await _eventService.GetPagedEventAsync(request.pageIndex, request.pageSize);

                //check for when result is null
                response.PageParams = pageparams;
                response.Response = _mapper.Map<IEnumerable<EventResponse>>(result);
                await _redisService.SetAsync(cacheKey, response, cacheTimeInMinutes: 2);
                return response;
            }
        }
    }
}
