using EventService.Domain.Interfaces;
using EventService.SharedLibrary.Exceptions;
using EventService.SharedLibrary.Model.ResponseModel;
using FluentValidation;
using MediatR;

namespace EventService.Application.UseCases.Events.Commands
{
    public class EventDelete
    {
        public record Command(Guid Id) : IRequest<Result<string>>;

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id)
                    .NotEmpty()
                    .WithMessage("Id is required.")
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid Guid.");
            }
        }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly IEventService _eventService;

            public Handler(IEventService eventService)
            {
                _eventService = eventService;
            }

            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var response = new Result<string>();
                var @event = await _eventService.GetByEventIdAsync(request.Id);

                if (@event == null)
                {
                    throw new EntityNotFoundException($"Event with {request.Id} does not exist");
                }

                await _eventService.RemoveEventAsync(@event);

                response.Response = "Delete successful";

                return response;
            }
        }
    }
}
