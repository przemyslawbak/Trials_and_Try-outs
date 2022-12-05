using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SimpleCommandDemoApp.ViewModels;

namespace SimpleCommandDemoApp.Commands.Specific
{
   public class DivisionCommand : ICommand
    {
        private CalculatorViewModel calculatorViewModel;
        public DivisionCommand(CalculatorViewModel vm) // Point 2
        {
            calculatorViewModel = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true; // Point 3
        }
        public void Execute(object parameter)
        {
            calculatorViewModel.Divide(); // Point 4
        }

        public event EventHandler CanExecuteChanged;
    }
}
