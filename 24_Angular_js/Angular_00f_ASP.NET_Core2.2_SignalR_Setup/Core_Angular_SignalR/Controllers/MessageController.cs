using Core_Angular_SignalR.Hubs;
using Core_Angular_SignalR.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Core_Angular_SignalR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly IHubContext<MessageHub> _hubContext;

        public MessageController(IHubContext<MessageHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost("messageArrived")]
        public async Task<IActionResult> MessageArrived([FromBody]Message inputModel)
        {
            await _hubContext.Clients.All.SendAsync("MessageReceived", inputModel);

            return StatusCode(200);
        }
    }
}
