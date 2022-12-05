using Microsoft.AspNetCore.Mvc;

namespace Other_04_Polly.Controllers
{
    //https://inthetechpit.com/2022/05/20/retry-and-circuit-breaker-policy-example-net-6-and-polly/
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {

        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            throw new Exception("Error");
        }

    }
}
