using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Services.Dialogs;

namespace Wpf_.ViewModels
{
    public class DialogViewModel : IDialogRequestClose
    {
        public DialogViewModel(string message)
        {
            Message = message;
            OkCommand = new DelegateCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            CancelCommand = new DelegateCommand(p => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
        }

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
        public string Message { get; }
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
    }
}
