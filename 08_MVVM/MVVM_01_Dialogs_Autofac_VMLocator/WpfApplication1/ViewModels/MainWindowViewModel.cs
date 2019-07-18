using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfApplication1.Dialogs.DialogFacade;

namespace WpfApplication1.ViewModels
{
    public class MainWindowViewModel
    {
        private IDialogFacade _dialogFacade;

        public IDialogFacade DialogFacade
        {
            get { return _dialogFacade; }
            set { _dialogFacade = value; }
        }

        public MainWindowViewModel(IDialogFacade dialogFacade)
        {
            _dialogFacade = dialogFacade;
            OpenDialogCommand = new RelayCommand(OnOpenDialog);
        }

        private void OnOpenDialog(object parameter)
        {
            var vm = new DialogYesNoViewModel("Question");
            vm.YesClicked += OptionYes;
            vm.NoClicked += OptionNo;
            _dialogFacade.ShowDialogYesNo2(vm);
        }

        private ICommand _openDialogCommand;
        public ICommand OpenDialogCommand { get; private set; }


        private void OptionYes(object sender, EventArgs e)
        {

        }

        private void OptionNo(object sender, EventArgs e)
        {

        }
    }
}
