using GettingStarted.Contracts;
using MassTransit;
using System.Threading.Tasks;

namespace GettingStarted.Controllers
{
    //To use the request client, a controller (or a consumer) uses the client via a constructor dependency.


    public class RequestController : Controller
    {
        IRequestClient<CheckOrderStatus> _client;

        public RequestController(IRequestClient<CheckOrderStatus> client)
        {
            _client = client;
        }

        public async Task<ActionResult> Get(string id)
        {
            var response = await _client.GetResponse<OrderStatusResult>(new { OrderId = id });

            return View(response.Message);
        }
    }
}
