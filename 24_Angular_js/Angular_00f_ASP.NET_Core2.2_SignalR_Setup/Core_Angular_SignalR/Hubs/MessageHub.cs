using Core_Angular_SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Core_Angular_SignalR.Hubs
{
    public class MessageHub : Hub
    {
        public async Task NewMessage(Message msg)
        {
            await Clients.All.SendAsync("MessageReceived", msg);
        }
    }
}
