using MVVM_DialogService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_DialogService.ViewModels
{
    //credits: https://www.youtube.com/watch?v=oeI2MmUELbY
    public class MainWindowViewModel
    {
        public ICommand YesNoCommand { get; private set; }
        public ICommand AlertCommand { get; private set; }

        public MainWindowViewModel()
        {
            YesNoCommand = new DelegateCommand(YesNo);
            AlertCommand = new DelegateCommand(Alert);
        }

        private void Alert(object obj)
        {
            throw new NotImplementedException(); //cos zrob..
        }

        private void YesNo(object obj)
        {
            throw new NotImplementedException(); //cos zrob..
        }
    }
}
