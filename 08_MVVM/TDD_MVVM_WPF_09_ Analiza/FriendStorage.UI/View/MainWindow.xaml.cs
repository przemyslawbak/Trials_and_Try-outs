using FriendStorage.UI.ViewModel;
using System.Windows;

namespace FriendStorage.UI.View
{
  public partial class MainWindow : Window
  {
    private MainViewModel _viewModel;

    public MainWindow(MainViewModel viewModel)
    {
      InitializeComponent();
      this.Loaded += MainWindow_Loaded;

      _viewModel = viewModel;
      DataContext = _viewModel;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      _viewModel.Load();
    }
  }
}
