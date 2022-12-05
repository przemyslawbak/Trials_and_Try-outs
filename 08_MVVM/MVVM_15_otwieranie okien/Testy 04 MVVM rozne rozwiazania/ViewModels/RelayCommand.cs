using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Testy_04_MVVM_rozne_rozwiazania.ViewModels
{
    public class RelayCommand : ICommand //klasa 
    {
        #region Fields
        //zmienne parametrów odbieranych z VM 
        readonly Action<object> _execute; //zmienna
        readonly Predicate<object> _canExecute; //zmienna
        #endregion // Fields


        #region Constructor
        //Konstruktor odbiera parametry z VM
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null) //konstruktor z 2 argumentami
                                                                                         //do argumentów się przekazuje metody
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructor


        #region ICommand Members
        [DebuggerStepThrough] //"Instructs the debugger to step through the code instead of stepping into the code"

        //implementacja interfejsu


        //mówi UI czy można, lub nie, wyegzekwować interfejs
        public bool CanExecute(object parameter) //metoda
        {
            return _canExecute == null ? true : _canExecute(parameter); //jeżeli _canExecute == null, to true, jeśłi nie, to _canExecute(parameter)
        }
        public event EventHandler CanExecuteChanged //event, jest wywoływany gdy własność CanExecute metody jest zmieniona
        {
            add
            {
                if (_canExecute != null) CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null) CommandManager.RequerySuggested -= value;
            }
        }

        //tutaj wszystko się dzieje
        public void Execute(object parameter) //metoda, odwołuje się CommandDialogBox w OknaDialogowe.cs
        {
            _execute(parameter); //wykonanie z przekazanym parametrem
        }
        #endregion // ICommand Members
    }
}
