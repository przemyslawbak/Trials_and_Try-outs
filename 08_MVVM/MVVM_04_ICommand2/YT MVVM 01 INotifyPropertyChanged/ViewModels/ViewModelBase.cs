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
        public SimpleCommand SimpleCommand { get; set; } //widok to widzi
        public ParameterCommand ParameterCommand { get; set; } //widok to widzi
        public ViewModelBase()
        {
            this.SimpleCommand = new SimpleCommand(this);
            this.ParameterCommand = new ParameterCommand(this);
        }

        public void SimpleMethod()
        {
            Debug.WriteLine("Hello");
        }
        public void ParameterMethod(string person)
        {
            Debug.WriteLine(person);
        }
    }
}
