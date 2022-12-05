using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleCommandDemoApp.Commands.Generic
{
    public class RelayCommand : ICommand
    {
        private Action commandTask; //zm

        public RelayCommand(Action workToDo) //konstr
        {
            commandTask = workToDo;
        }

        //implementacja interfejsu

        public bool CanExecute(object parameter) // (czy?)zawsze można wyegzekwować interfejs
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) //wykonanie
        {
            commandTask();
        }
    }
}
