

namespace Wpf_
{
    using System.Windows;
    using Wpf_.ViewModels;
    using Wpf_.Views;
    using Wpf_Services.Dialogs;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            IDialogService dialogService = new DialogService(MainWindow);

            dialogService.Register<DialogViewModel, DialogWindow>();

            var viewModel = new MainWindowViewModel(dialogService);
            var view = new MainWindow { DataContext = viewModel };

            view.ShowDialog();
        }
    }
}
