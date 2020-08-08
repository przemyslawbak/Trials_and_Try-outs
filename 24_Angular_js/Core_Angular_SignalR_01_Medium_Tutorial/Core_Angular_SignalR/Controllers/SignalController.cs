using Core_Angular_SignalR.Hubs;
using Core_Angular_SignalR.Models;
using Core_Angular_SignalR.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Core_Angular_SignalR.Controllers
{
    [Route("api/v1/signals")]
    [ApiController]
    public class SignalController : ControllerBase
    {
        private readonly ISignalService _signalService;
        private readonly IHubContext<SignalHub> _hubContext;

        public SignalController(ISignalService signalService, IHubContext<SignalHub> hubContext)
        {
            _signalService = signalService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("deliverypoint")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> SignalArrived(SignalInputModel inputModel)
        {
            var saveResult = await _signalService.SaveSignalAsync(inputModel);

            if (saveResult)
            {
                SignalViewModel signalViewModel = new SignalViewModel()
                {
                    Description = inputModel.Description,
                    CustomerName = inputModel.CustomerName,
                    Area = inputModel.Area,
                    Zone = inputModel.Zone,
                    SignalStamp = Guid.NewGuid().ToString()
                };

                await _hubContext.Clients.All.SendAsync("SignalMessageReceived", "some message");
            }

            return StatusCode(200, saveResult);
        }
    }
}
