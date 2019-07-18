using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SimpleCommandDemoApp.ViewModels;

namespace SimpleCommandDemoApp.Commands.Specific
{
   public class MultiplicationCommand : ICommand
    {
        private CalculatorViewModel calculatorViewModel;
        public MultiplicationCommand(CalculatorViewModel vm) // Point 2
        {
            calculatorViewModel = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true; // Point 3
        }
        public void Execute(object parameter)
        {
            calculatorViewModel.Multiply(); // Point 4
        }
        
        public event EventHandler CanExecuteChanged;
    }
}
