using Financial.Commands;
using Financial.Services;
using System;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class DialogViewModel : IDialogViewModel
    {
        private readonly IWindowManager _winService;

        public DialogViewModel(IWindowManager winService)
        {
            _winService = winService;

            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);
        }

        public ICommand OkCommand { get; } //on OK click
        public ICommand CancelCommand { get; } //on Cancel click

        public bool? DialogResult { get; set; }

        private void Ok(object obj)
        {
            DialogResult = true;

            _winService.CloseWindow(this);
        }

        private void Cancel(object obj)
        {
            DialogResult = false;

            _winService.CloseWindow(this);
        }
    }
}
