//https://inthetechpit.com/2022/05/20/retry-and-circuit-breaker-policy-example-net-6-and-polly/

using Polly;

var builder = WebApplication.CreateBuilder(args);
var httpRetryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
    .CircuitBreakerAsync(2, TimeSpan.FromSeconds(30));

/*
 * The above line will open the circuit and will start giving Circuit Open Exception
 * after 2 attempts and keep the circuit Open for 30 seconds,
 * which means you cannot make further calls to the API for that duration.
 * */

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient("errorApi", c => { c.BaseAddress = new Uri("https://localhost:7250"); });
builder.Services.AddSingleton<IAsyncPolicy<HttpResponseMessage>>(httpRetryPolicy);


var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
