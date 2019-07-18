using System.Windows;
using WpfApplication1.Dialogs.DialogService;
using WpfApplication1.ViewModels;

namespace WpfApplication1.Dialogs.DialogFacade
{
    public interface IDialogFacade
    {
        Window Owner { get; set; }

        DialogResult ShowDialogYesNo(string message);
        void ShowDialogYesNo2(DialogYesNoViewModel vm);
    }
}