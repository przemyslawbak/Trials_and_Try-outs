using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SimpleCommandDemoApp.Commands.Generic;
using SimpleCommandDemoApp.Commands.Specific;

namespace SimpleCommandDemoApp.ViewModels
{
    public class CalculatorViewModel : ViewModelBase //przez dziedziczenie bierze INotifyPropertyChanged
    {
        #region Private Fields

        //zmienne

        private double firstValue;
        private double secondValue;
        private double output;
        private PlusCommand plusCommand;
        private MinusCommand minusCommand;
        private MultiplicationCommand multiplicationCommand;
        private DivisionCommand divisionCommand;
        #endregion

        //konstruktor
        public CalculatorViewModel()
        {
            plusCommand = new PlusCommand(this);
            minusCommand = new MinusCommand(this);
            multiplicationCommand = new MultiplicationCommand(this);
            divisionCommand = new DivisionCommand(this);
        }

        #region Public Properties

        //własności
        public double FirstValue
        {
            get { return firstValue; }

            set
            {
                firstValue = value;
                OnPropertyChanged("FirstValue");
            }
        }
        public double SecondValue
        {
            get { return secondValue; }
            set
            {
                secondValue = value;
                OnPropertyChanged("SecondValue");
            }
        }
        public double Output
        {

            get { return output; }

            set
            {
                output = value;
                OnPropertyChanged("Output");
            }
        }

        #endregion

        #region Commands

        public ICommand AddCommand
        {
            get
            {
                return plusCommand;
                //return new RelayCommand(Add);
            }
        }

        public ICommand SubstractCommand
        {
            get
            {
                return minusCommand;
                // return new RelayCommand(Substract);
            }

        }

        public ICommand MultiplyCommand // Point 3
        {
            get
            {
                return multiplicationCommand;
                // return new RelayCommand(Multiply);
            }
        }

        public ICommand DivideCommand // Point 3
        {
            get
            {
                return divisionCommand;
                // return new RelayCommand(Divide);
            }
        }

        #endregion

        #region Methods

        internal void Add()
        {
            Output = firstValue + secondValue;

        }

        internal void Substract()
        {
            Output = firstValue - secondValue;
        }

        internal void Multiply()
        {
            Output = firstValue * secondValue;
        }

        internal void Divide()
        {
            Output = firstValue / secondValue;
        }

        #endregion

    }
}
