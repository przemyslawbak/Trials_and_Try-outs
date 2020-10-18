using System;

namespace BasicConfig.Models.ViewModels
{
    public class StatsViewModel
    {
        public DateTime Date { get; set; }
        public int Moving { get; set; }
        public int NotMoving { get; set; }
        public int Missing { get; set; }
        public int Expired { get; set; }
    }
}
