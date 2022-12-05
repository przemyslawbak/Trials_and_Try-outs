using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CodeProject_01_ICommand
{
    class MainWindowViewModel
    {
        private ICommand hiButtonCommand;

        private ICommand toggleExecuteCommand { get; set; }

        private bool canExecute = true;

        public string HiButtonContent
        {
            get
            {
                return "click to hi";
            }
        }

        public bool CanExecute
        {
            get
            {
                return this.canExecute;
            }

            set
            {
                if (this.canExecute == value)
                {
                    return;
                }

                this.canExecute = value;
            }
        }

        public ICommand ToggleExecuteCommand
        {
            get
            {
                return toggleExecuteCommand;
            }
            set
            {
                toggleExecuteCommand = value;
            }
        }

        public ICommand HiButtonCommand
        {
            get
            {
                return hiButtonCommand;
            }
            set
            {
                hiButtonCommand = value;
            }
        }

        public MainWindowViewModel()
        {
            HiButtonCommand = new RelayCommand(ShowMessage, param => this.canExecute);
            toggleExecuteCommand = new RelayCommand(ChangeCanExecute);
        }

        public void ShowMessage(object obj)
        {
            MessageBox.Show(obj.ToString());
        }

        public void ChangeCanExecute(object obj)
        {
            canExecute = !canExecute;
        }
    }
}
