//tutorial: https://khalidabuhakmeh.com/testing-aspnet-core-6-apps

using Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<NameService>();

var app = builder.Build();

app.MapGet("/", (IMessageService message, NameService names)
    => $"{names.Name} says \"{message.SayHello()}\"");

app.Run();

//https://stackoverflow.com/a/70026704/12603542
public partial class Program 
{
}