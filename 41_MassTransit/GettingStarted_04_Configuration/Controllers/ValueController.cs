using MassTransit;
using System.Threading.Tasks;

namespace GettingStarted.Controllers
{
    public class ValueController : Controller
    {
        readonly IPublishEndpoint _publishEndpoint;

        public ValueController(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<ActionResult> Post(string value)
        {
            await _publishEndpoint.Publish<ValueEntered>(new
            {
                Value = value
            });

            return Ok();
        }

    }
}
