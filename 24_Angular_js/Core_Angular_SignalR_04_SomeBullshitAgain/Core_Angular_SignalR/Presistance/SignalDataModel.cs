using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core_Angular_SignalR.Presistance
{
    public class SignalDataModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public string AccessCode { get; set; }
        public string Area { get; set; }
        public string Zone { get; set; }
        public DateTime SignalTime { get; set; }
    }
}
