using EventService.SharedLibrary.Exceptions;
using EventService.SharedLibrary.Model.ResponseModel;
using System.Net;
using System.Text.Json;

namespace EventService.API.Extensions
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var problemDetails = new Result<string>
            {
                ErrorFlag = true,
                Response = exception.Message,
                Message = exception.Message,
                Title = exception.Message,
                Detail = exception.StackTrace,
                Status = StatusCodes.Status500InternalServerError,
            };

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    problemDetails.Detail = JsonSerializer.Serialize(new { validationException.Failures });
                    problemDetails.Status = StatusCodes.Status400BadRequest;
                    break;
                case EntityNotFoundException entityNotFoundException:
                    code = HttpStatusCode.NotFound;
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    problemDetails.Detail = string.IsNullOrEmpty(entityNotFoundException.Message) ? "Entity not found.Please provide a valid search criteria and try again." : entityNotFoundException.Message;
                    problemDetails.Status = StatusCodes.Status404NotFound;                    
                    break;                
                default:
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = (int)code;
                    problemDetails.Detail = "An error occured on the payload";
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
