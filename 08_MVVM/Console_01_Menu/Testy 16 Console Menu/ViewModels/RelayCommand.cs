using System;
using System.Collections.Generic;
using System.Text;
using Testy_16_Console_Menu.Models;

namespace Testy_16_Console_Menu.ViewModels
{
    class RelayCommand : ICommand
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
