using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testy_04_Bare_Metal_MVVM.Models;

namespace Testy_04_Bare_Metal_MVVM.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action executeAction;
        public RelayCommand(Action execute)
        {
            executeAction = execute;
        }
        public void Execute()
        {
            executeAction();
        }
    }
}
