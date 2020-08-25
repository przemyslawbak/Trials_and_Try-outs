using System;

namespace Core_Angular_SignalR.Models
{
    public class Message
    {
        public string ClientUniqueId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}
