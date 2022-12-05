using Financial.Commands;
using Financial.Models;
using Financial.Services;
using System.Windows.Input;

namespace Financial.ViewModels
{

    public interface IDialogVM
    {

    }

    public class DialogViewModel : IDialogVM, IResultViewModel, IModalDialogViewModel
    {
        private readonly IWindowManager _winService;

        public DialogViewModel(IWindowManager winService)
        {
            _winService = winService;

            SomeString = "dupa";

            OkCommand = new DelegateCommand(Ok);
            CancelCommand = new DelegateCommand(Cancel);

            DialogResult = null;
        }

        public bool? DialogResult { get; set; }
        public object ObjectResult { get; set; }

        public string SomeString { get; set; }
        public ICommand OkCommand { get; } //on OK click
        public ICommand CancelCommand { get; } //on Cancel click

        private void Ok(object obj)
        {
            //DialogResult = true;

            ObjectResult = new Stock()
            {
                Ticker = "dupa"
            };

            _winService.CloseWindow(this);
        }

        private void Cancel(object obj)
        {
            //DialogResult = false;

             ObjectResult = null;

            _winService.CloseWindow(this);
        }
    }
}
