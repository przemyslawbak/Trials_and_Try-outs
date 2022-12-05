using Financial.Commands;
using Financial.Models;
using Financial.Services;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class DialogViewModel : IResultViewModel
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

        public object ObjectResult { get; set; }

        private void Ok(object obj)
        {
            ObjectResult = new Stock()
            {
                Ticker = "DUPA"
            };

            _winService.CloseWindow(this);
        }

        private void Cancel(object obj)
        {
            ObjectResult = false;

            _winService.CloseWindow(this);
        }
    }
}
