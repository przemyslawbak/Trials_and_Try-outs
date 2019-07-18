using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfApplication1.Dialogs.DialogService;

namespace WpfApplication1.ViewModels
{
    public class DialogYesNoViewModel : DialogViewModelBase
    {
        public event EventHandler YesClicked = delegate { };
        public event EventHandler NoClicked = delegate { };

        private ICommand yesCommand = null;
        public ICommand YesCommand
        {
            get { return yesCommand; }
            set { yesCommand = value; }
        }

        private ICommand noCommand = null;
        public ICommand NoCommand
        {
            get { return noCommand; }
            set { noCommand = value; }
        }

        public DialogYesNoViewModel(string message)
            : base(message)
        {
            this.yesCommand = new RelayCommand(OnYesClicked);
            this.noCommand = new RelayCommand(OnNoClicked);
        }

        private void OnYesClicked(object parameter)
        {
            this.YesClicked(this, EventArgs.Empty);
            this.CloseDialogWithResult(parameter as Window, DialogResult.Yes);
        }

        private void OnNoClicked(object parameter)
        {
            this.NoClicked(this, EventArgs.Empty);
            this.CloseDialogWithResult(parameter as Window, DialogResult.No);
        }
    }
}
