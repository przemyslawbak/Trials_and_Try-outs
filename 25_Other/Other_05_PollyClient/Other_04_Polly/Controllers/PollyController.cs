using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;

namespace Other_04_Polly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PollyController : ControllerBase
    {

        private readonly ILogger<PollyController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAsyncPolicy<HttpResponseMessage> _policy;

        public PollyController(ILogger<PollyController> logger, IHttpClientFactory httpClientFactory, IAsyncPolicy<HttpResponseMessage> policy)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _policy = policy;
        }

        [HttpGet(Name = "GetApiResult")]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var client = _httpClientFactory.CreateClient("errorApi");
            var response = await _policy.ExecuteAsync(() => client.GetAsync("values"));
            response.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<string[]>(await response.Content.ReadAsStringAsync());
        }

    }
}
