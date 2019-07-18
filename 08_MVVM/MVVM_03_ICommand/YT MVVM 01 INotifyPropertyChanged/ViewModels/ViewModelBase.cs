using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT_MVVM_01_INotifyPropertyChanged.ViewModels.Commands;

namespace YT_MVVM_01_INotifyPropertyChanged.ViewModels
{
    public class ViewModelBase
    {
        public SimpleCommand SimpleCommand { get; set; }
        public ViewModelBase()
        {
            this.SimpleCommand = new SimpleCommand(this);
        }

        public void SimpleMethod()
        {
            Debug.WriteLine("Hello");
        }
    }
}
