using MVVM_09_Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_09_Prism.ViewModels
{
    public class MainViewModel
    {
        public void UpdateName(string name)
        {
            Utility.EventAggregator.GetEvent<UpdateNameEvent>().Publish(name);
        }
    }
}
