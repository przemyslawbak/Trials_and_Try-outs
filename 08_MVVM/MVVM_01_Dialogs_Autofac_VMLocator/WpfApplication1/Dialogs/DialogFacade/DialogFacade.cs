using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using WpfApplication1.Dialogs.DialogService;
using WpfApplication1.ViewModels;
using WpfApplication1.Views;

namespace WpfApplication1.Dialogs.DialogFacade
{
    public class DialogFacade : Control, IDialogFacade
    {
        public Window Owner { get; set; }

        public DialogFacade()
        {

        }

        public DialogResult ShowDialogYesNo(string message)
        {
            DialogViewModelBase vm = new DialogYesNoViewModel(message);
            return this.ShowDialog(vm);
        }

        public void ShowDialogYesNo2(DialogYesNoViewModel vm)
        {
            this.ShowDialog(vm);
        }

        private DialogResult ShowDialog(DialogViewModelBase vm)
        {
            DialogWindow win = new DialogWindow();
            if (this.Owner != null)
                win.Owner = this.Owner;
            win.DataContext = vm;
            win.ShowDialog();
            DialogResult result =
                (win.DataContext as DialogViewModelBase).UserDialogResult;
            return result;
        }
    }
}
