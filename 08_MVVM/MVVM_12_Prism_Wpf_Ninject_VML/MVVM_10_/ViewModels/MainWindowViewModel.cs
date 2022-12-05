using MVVM_10_.Events;
using MVVM_10_.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM_10_.ViewModels
{
    //https://stackoverflow.com/questions/25366291/how-to-handle-dependency-injection-in-a-wpf-mvvm-application
    public class MainWindowViewModel
    {
        IEventAggregator _eventAggregator;
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            UpdateName("Name1");
        }

        public void UpdateName(string name)
        {

            ChildView1 win1 = new ChildView1();
            win1.Show();

            _eventAggregator.GetEvent<UpdateNameEvent>().Publish(name);
        }
    }
}
