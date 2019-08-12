using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using System;
using System.Windows;
using System.Windows.Input;
using Wpf_.Commands;
using Wpf_.Views;

namespace Wpf_.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        private string _path;
        private string _dialogText;
        private string _confirmation;

        public MainViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;

            OpenFileCommand = new DelegateCommand(OnOpenFile);
            ImplicitShowDialogCommand = new DelegateCommand(ImplicitShowDialog);
            ExplicitShowDialogCommand = new DelegateCommand(ExplicitShowDialog);
            ShowMessageBoxWithMessageCommand = new DelegateCommand(ShowMessageBoxWithMessage);
            ShowMessageBoxWithCaptionCommand = new DelegateCommand(ShowMessageBoxWithCaption);
            ShowMessageBoxWithButtonCommand = new DelegateCommand(ShowMessageBoxWithButton);
            ShowMessageBoxWithIconCommand = new DelegateCommand(ShowMessageBoxWithIcon);
            ShowMessageBoxWithDefaultResultCommand = new DelegateCommand(ShowMessageBoxWithDefaultResult);
        }

        public ICommand OpenFileCommand { get; }
        public ICommand ImplicitShowDialogCommand { get; }
        public ICommand ExplicitShowDialogCommand { get; }
        public ICommand ShowMessageBoxWithMessageCommand { get; }

        public ICommand ShowMessageBoxWithCaptionCommand { get; }

        public ICommand ShowMessageBoxWithButtonCommand { get; }

        public ICommand ShowMessageBoxWithIconCommand { get; }

        public ICommand ShowMessageBoxWithDefaultResultCommand { get; }
        public string Confirmation
        {
            get => _confirmation;
            private set
            {
                _confirmation = value;
                OnPropertyChanged();
            }
        }
        public string DirPath
        {
            get => _path;
            set
            {
                _path = value;
                OnPropertyChanged();
            }
        }
        public string DialogText
        {
            get => _dialogText;
            set
            {
                _dialogText = value;
                OnPropertyChanged();
            }
        }

        public void OnOpenFile(object obj)
        {
            var settings = new OpenFileDialogSettings
            {
                Title = "This Is The Title",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            bool? success = _dialogService.ShowOpenFileDialog(this, settings);
            if (success == true)
            {
                DirPath = settings.FileName;
            }
        }
        private void ImplicitShowDialog(object obj)
        {
            ShowDialog(viewModel => _dialogService.ShowDialog(this, viewModel));
        }

        private void ExplicitShowDialog(object obj)
        {
            ShowDialog(viewModel => _dialogService.ShowDialog<AddTextDialog>(this, viewModel));
        }
        private void ShowDialog(Func<AddTextDialogViewModel, bool?> showDialog)
        {
            var dialogViewModel = new AddTextDialogViewModel();

            bool? success = showDialog(dialogViewModel);
            if (success == true)
            {
                DialogText = dialogViewModel.Text;
            }
        }
        private void ShowMessageBoxWithMessage(object obj)
        {
            MessageBoxResult result = _dialogService.ShowMessageBox(
                this,
                "This is the text.");

            UpdateResult(result);
        }

        private void ShowMessageBoxWithCaption(object obj)
        {
            MessageBoxResult result = _dialogService.ShowMessageBox(
                this,
                "This is the text.",
                "This Is The Caption");

            UpdateResult(result);
        }

        private void ShowMessageBoxWithButton(object obj)
        {
            MessageBoxResult result = _dialogService.ShowMessageBox(
                this,
                "This is the text.",
                "This Is The Caption",
                MessageBoxButton.OKCancel);

            UpdateResult(result);
        }

        private void ShowMessageBoxWithIcon(object obj)
        {
            MessageBoxResult result = _dialogService.ShowMessageBox(
                this,
                "This is the text.",
                "This Is The Caption",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information);

            UpdateResult(result);
        }

        private void ShowMessageBoxWithDefaultResult(object obj)
        {
            MessageBoxResult result = _dialogService.ShowMessageBox(
                this,
                "This is the text.",
                "This Is The Caption",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Information,
                MessageBoxResult.Cancel);

            UpdateResult(result);
        }

        private void UpdateResult(MessageBoxResult result)
        {
            switch (result)
            {
                case MessageBoxResult.OK:
                    Confirmation = "We got confirmation to continue!";
                    break;

                case MessageBoxResult.Cancel:
                    Confirmation = string.Empty;
                    break;

                default:
                    throw new NotSupportedException($"{_confirmation} is not supported.");
            }
        }
    }
}
