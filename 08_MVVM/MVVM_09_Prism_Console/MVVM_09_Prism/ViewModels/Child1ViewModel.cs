using MVVM_09_Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_09_Prism.ViewModels
{
    public class Child1ViewModel
    {
        public string Name { get; set; }

        public Child1ViewModel()
        {
            Utility.EventAggregator.GetEvent<UpdateNameEvent>().Subscribe(UpdateName);
        }

        private void UpdateName(string name)
        {
            this.Name = name;
        }
    }
}
