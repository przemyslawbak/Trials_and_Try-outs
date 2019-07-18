using MVVM_10_.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_10_.ViewModels
{
    public class MainViewModel
    {
        Child1ViewModel child1;
        Child2ViewModel child2;
        public MainViewModel()
        {
            child1 = new Child1ViewModel();
            child2 = new Child2ViewModel();
            UpdateName("Name1");
        }

        public void UpdateName(string name)
        {
            Utility.EventAggregator.GetEvent<UpdateNameEvent>().Publish(name);

            MessageBox.Show(child1.Name);
            MessageBox.Show(child2.Name);
        }
    }
}
