using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_10_.Events
{
    public class Utility
    {
        public static EventAggregator EventAggregator { get; set; }

        static Utility()
        {
            EventAggregator = new EventAggregator();
        }
    }
}
