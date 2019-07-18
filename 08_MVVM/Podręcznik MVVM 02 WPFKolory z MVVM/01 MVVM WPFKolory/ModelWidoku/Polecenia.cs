using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace _01_MVVM_WPFKolory.ModelWidoku
{
    public class ResetujCommand : ICommand
    {
        private readonly EdycjaKoloru modelWidoku;
        public ResetujCommand(EdycjaKoloru modelWidoku)
        {
            this.modelWidoku = modelWidoku;
        }
        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }
        public bool CanExecute(object parameter)
        {
            return (modelWidoku.R != 0) || (modelWidoku.G != 0) || (modelWidoku.B != 0);
        }
        public void Execute(object parameter)
        {
            if (modelWidoku != null)
            {
                modelWidoku.R = 0;
                modelWidoku.G = 0;
                modelWidoku.B = 0;
            }
        }
    }
}
