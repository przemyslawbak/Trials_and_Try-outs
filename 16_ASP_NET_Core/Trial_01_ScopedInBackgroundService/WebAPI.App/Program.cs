using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using WebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHostedService<ScrapTimer>();
builder.Services.AddScoped<IScopedProcessingService, ScopedSimpleTextService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();