using MVVM_10_.Commands;
using MVVM_10_.Events;
using MVVM_10_.Views;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM_10_.ViewModels
{
    //problem with resolving of VM: https://stackoverflow.com/questions/25366291/how-to-handle-dependency-injection-in-a-wpf-mvvm-application
    public class MainWindowViewModel
    {
        IEventAggregator _eventAggregator;
        public MainWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            ClickCommand = new DelegateCommand(OnClickDataExecute);
        }

        private void OnClickDataExecute(object obj)
        {
            ChildView1 win1 = new ChildView1();
            win1.Show();

            _eventAggregator.GetEvent<UpdateNameEvent>().Publish("Name1");
        }

        public ICommand ClickCommand { get; private set; }
    }
}
