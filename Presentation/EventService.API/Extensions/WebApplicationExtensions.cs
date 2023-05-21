
using EventService.Application.UseCases.Events.Commands;
using EventService.Application.UseCases.Events.Queries;
using EventService.Application.UseCases.Events.ViewModels;
using EventService.SharedLibrary.Model.ResponseModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventService.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static void AddAppRoutes(this WebApplication app)
        {
            app.MapPost("api/v1/events", async ([FromBody] EventCreate.Command command, ISender sender)
                            => await sender.Send(command))
                            .WithTags("Events")
                            .Produces(200, typeof(Result<EventData>))
                            .Produces(404, typeof(Result<string>))
                            .Produces(400, typeof(Result<string>))
                            .Produces(500, typeof(Result<string>));


            app.MapPut("api/v1/events/{eventId:guid:required}", async (Guid eventId, [FromBody] EventData command, ISender sender)
                            => await sender.Send(new EventEdit.Command(command, eventId)))
                            .WithTags("Events")
                            .Produces(200, typeof(Result<EventData>))
                            .Produces(404, typeof(Result<string>))
                            .Produces(400, typeof(Result<string>))
                            .Produces(500, typeof(Result<string>));

            app.MapGet("api/v1/events/{eventId:guid:required}", async (Guid eventId, ISender sender)
                           => await sender.Send(new EventDetail.Query(eventId)))
                           .WithTags("Events")
                           .Produces(200, typeof(Result<EventData>))
                           .Produces(404, typeof(Result<string>))
                           .Produces(400, typeof(Result<string>))
                           .Produces(500, typeof(Result<string>));


            app.MapGet("api/v1/events", async ([FromQuery] int pageIndex, [FromQuery] int pageSize, ISender sender)
                           => await sender.Send(new EventListQuery.Query(pageIndex, pageSize)))
                           .WithTags("Events")
                           .Produces(200, typeof(PaginatedResult<IEnumerable<EventData>>))
                           .Produces(404, typeof(Result<string>))
                           .Produces(400, typeof(Result<string>))
                           .Produces(500, typeof(Result<string>));

            app.MapDelete("api/v1/events/{eventId:guid:required}", async (Guid eventId, ISender sender)
                           => await sender.Send(new EventDelete.Command(eventId)))
                           .WithTags("Events")
                           .Produces(200, typeof(Result<string>))
                           .Produces(404, typeof(Result<string>))
                           .Produces(400, typeof(Result<string>))
                           .Produces(500, typeof(Result<string>));
        }
    }
}
