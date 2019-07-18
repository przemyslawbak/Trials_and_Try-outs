using _01_MVVM_WPFKolory.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace _01_MVVM_WPFKolory.ModelWidoku
{

    using Model;

    public class RelayCommand : ICommand
    {
        #region Fields
        readonly Action<object> _execute;
        readonly Predicate<object> _canExecute;
        #endregion // Fields
        #region Constructor
        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            if (execute == null) throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructor
        #region ICommand Members
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
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
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        #endregion // ICommand Members
    }


    public class EdycjaKoloru : INotifyPropertyChanged
    {
        private readonly Kolor kolor = Ustawienia.Czytaj();

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(params string[] nazwyWłasności) //metoda pomocnicza
        {
            if (PropertyChanged != null)
            {
                foreach (string nazwaWłasności in nazwyWłasności)
                    PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
            }
        }

        private ICommand zapiszCommand;
        public ICommand Zapisz
        {
            get
            {
                if (zapiszCommand == null)
                    zapiszCommand = new RelayCommand(argument =>
                    {
                        Ustawienia.Zapisz(kolor);
                    });
                return zapiszCommand;
            }
        }

        private ICommand resetujCommand;
        public ICommand Resetuj
        {
            get
            {
                if (resetujCommand == null)
                {
                    resetujCommand = new RelayCommand(
                    argument =>
                    {
                        R = 0;
                        G = 0;
                        B = 0;
                    },
                    argument => (R != 0) || (G != 0) || (B != 0)
                    );
                }
                return resetujCommand;
            }
        }
        public byte R
        {
            get
            {
                return kolor.R;
            }
            set
            {
                kolor.R = value;
                OnPropertyChanged("R", "Color");
            }
        }
        public byte G
        {
            get
            {
                return kolor.G;
            }
            set
            {
                kolor.G = value;
                OnPropertyChanged("G", "Color");
            }
        }
        public byte B
        {
            get
            {
                return kolor.B;
            }
            set
            {
                kolor.B = value;
                OnPropertyChanged("B", "Color");
            }
        }
        public Color Color
        {
            get
            {
                return kolor.ToColor();
            }
        }

    }
    static class Rozszerzenia
    {
        public static Color ToColor(this Kolor kolor)
        {
            return new Color()
            {
                A = 255,
                R = kolor.R,
                G = kolor.G,
                B = kolor.B
            };
        }
    }
}
