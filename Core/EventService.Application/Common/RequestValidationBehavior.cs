using FluentValidation;
using MediatR;

namespace AaasSettingsService.Application.Common.Behavior
{
    public sealed class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> validators;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestValidationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">IEnumerable IValidator TRequest validators.</param>
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => this.validators = validators;

        /// <summary>
        /// .
        /// </summary>
        /// <param name="request">request.</param>
        /// <param name="next">next.</param>
        /// <param name="cancellationToken">cancellationToken.</param>
        /// <returns>TResponse.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var validationResults = await Task.WhenAll(this.validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var failures = validationResults.SelectMany(result => result.Errors).Where(f => f != null).ToList();

            if (failures.Count != 0)
            {
                throw new ValidationException(failures);
            }

            return await next();
        }
    }
}
