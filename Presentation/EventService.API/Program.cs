using EventService.API.Extensions;
using EventService.Application.Extensions;
using EventService.Domain.Interfaces;
using EventService.Infrastructure.Extensions;
using EventService.Persistence.Extensions;
using EventService.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddApplicationServices()
    .AddInfrastructureServices()
    .AddPersistenceServices(builder.Configuration);

var app = builder.Build();


var scope = app.Services.CreateScope();
var hostingEnvironment = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

if (!hostingEnvironment.IsEnvironment("Test"))
{
    var context = scope.ServiceProvider.GetRequiredService<IEventDbContext>();
    await context.Database.MigrateAsync();
    await Seeder.SeedDataAsync(context);
}


app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors(builder =>
               builder
                   .AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod());


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddAppRoutes();

app.Run();

public partial class Program { }