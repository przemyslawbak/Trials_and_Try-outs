using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Users.Models;

namespace Users.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(FancyModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
