using FriendStorage.UI.ViewModel;
using System;
using System.Windows;

namespace FriendStorage.UI.View
{
  public partial class MainWindow : Window
  {
        private MainViewModel _viewModel; //przypisanie pola do VM

        public MainWindow(MainViewModel viewModel)
    {
      InitializeComponent();
            this.Loaded += MainWindow_Loaded; //ewent wczytania widoku
            _viewModel = viewModel; //przekazanie VM do widoku
            DataContext = _viewModel; //przypisanie kontekstu danych do VM
    }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }
    }
}
