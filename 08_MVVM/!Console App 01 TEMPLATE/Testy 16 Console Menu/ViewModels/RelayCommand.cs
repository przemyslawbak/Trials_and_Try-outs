using System;
using System.Collections.Generic;
using System.Text;
using Testy_16_Console_Menu.Models;

namespace Testy_16_Console_Menu.ViewModels
{
    //we pass in an Action that we want the command to perform and when we call Execute, it executes the action
    //VM layer for command execution
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
