using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using SimpleCommandDemoApp.ViewModels;

namespace SimpleCommandDemoApp.Commands.Specific
{
   public class MinusCommand : ICommand
    {
        private CalculatorViewModel calculatorViewModel;
        public MinusCommand(CalculatorViewModel vm) 
        {
            calculatorViewModel = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true; 
        }
        public void Execute(object parameter)
        {
            calculatorViewModel.Substract(); 
        }

        public event EventHandler CanExecuteChanged;
    }
}
