//tutorial: https://www.claudiobernasconi.ch/2022/03/03/integration-testing-asp-net-core-6-webapi-applications/

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/sum", (int? n1, int? n2) => n1 + n2);

app.Run();

//https://stackoverflow.com/a/70026704/12603542
public partial class Program 
{
}