using Financial.Commands;
using Financial.Services;
using System.Windows.Input;

namespace Financial.ViewModels
{
    public class DialogViewModel
    {
        private readonly IWindowManager _winService;

        public DialogViewModel(IWindowManager winService)
        {
            _winService = winService;

            OkCommand = new DelegateCommand(Ok);
        }

        public ICommand OkCommand { get; } //on OK click

        public bool? DialogResult { get; set; }

        private void Ok(object obj)
        {
            DialogResult = true;

            _winService.CloseWindow(this);
        }
    }
}
