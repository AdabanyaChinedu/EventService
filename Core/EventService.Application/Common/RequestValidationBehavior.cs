using FluentValidation;
using MediatR;

namespace EventService.Application.Common.Behavior
{
    public sealed class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

       public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => this.validators = validators;

        
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(result => result.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new SharedLibrary.Exceptions.ValidationException(failures);
            }

            return await next();
        }
    }
}
