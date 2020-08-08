using System;

namespace Core_Angular_SignalR.Models
{

    public class Message
    {
        public string clientuniqueid { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
    }

}
