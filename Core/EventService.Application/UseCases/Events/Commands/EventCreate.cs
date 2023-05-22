using AutoMapper;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.Domain.Entities;
using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Constants;
using EventService.SharedLibrary.Model.ResponseModel;
using FluentValidation;
using MediatR;
using NodaTime;

namespace EventService.Application.UseCases.Events.Commands
{
    public class EventCreate
    {
        public class EventDataValidator : AbstractValidator<EventData>
        {
            public EventDataValidator()
            {

                RuleFor(x => x.Title)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Title is required")
                    .MaximumLength(EventConstants.MaxTitleLength)
                    .WithMessage($"Description max length {EventConstants.MaxTitleLength} exceeded");

                RuleFor(x => x.Description)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("Description is requred")
                    .MaximumLength(EventConstants.MaxDescriptionLength)
                    .WithMessage($"Description max length {EventConstants.MaxDescriptionLength} exceeded");


                RuleFor(x => x.UserId)
                    .NotEmpty()
                    .NotEqual(default(int))
                    .WithMessage("The value must be a valid non-default integer")
                    .InclusiveBetween(1, 10)
                    .WithMessage("Invalid UserId. Must be between 1 and 10");

                RuleFor(x => x.TimeZone)
                    .NotNull()
                    .NotEmpty()
                    .WithMessage("TimeZone is requred")
                    .Must(ValidTimezone)
                    .WithMessage("Invalid timezone");


                RuleFor(x => x.StartDate)
                .NotEqual(default(DateTime))
                .WithMessage("Start date is required.")
                .Must(BeNowOrFutureDate)
                .WithMessage("Invalid start date. Start Date must be a today or future date");

                RuleFor(x => x.EndDate)
                .NotEqual(default(DateTime))
                .WithMessage("End date is required.")
                .GreaterThan(x => x.StartDate)
                .WithMessage("End date must be later than the start date.");

            }
            private bool ValidTimezone(string timeZone)
            {
                var timeZoneProvider = DateTimeZoneProviders.Tzdb;

                return timeZoneProvider.GetZoneOrNull(timeZone) != null;
            }

            private bool BeNowOrFutureDate(DateTime date)
            {
                return date.Date >= DateTime.Today;
            }

        }

        public record Command(EventData Event) : IRequest<Result<EventUserResponse>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Event).NotNull().SetValidator(new EventDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<EventUserResponse>>
        {
            private readonly IEventService _eventService;
            private readonly IMapper _mapper;
            private readonly IHttpService _httpService;

            public Handler(IEventService eventService, IMapper mapper, IHttpService httpService)
            {
                _eventService = eventService;
                _mapper = mapper;
                _httpService = httpService;
            }
            public async Task<Result<EventUserResponse>> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new Result<EventUserResponse>();

                var @event = _mapper.Map<Event>(request.Event);

                await _eventService.AddEventAsync(@event);

                var eventRsp = _mapper.Map<EventUserResponse>(@event);

                eventRsp.user = await _httpService.GetAsync<UserData>($"https://jsonplaceholder.typicode.com/users/{request.Event.UserId}");

                response.Response = eventRsp;

                return response;
            }
        }

        
    }
}
