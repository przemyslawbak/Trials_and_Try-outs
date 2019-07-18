using Autofac;
using FriendStorage.UI.Startup;
using FriendStorage.UI.ViewModel;
using System;
using System.Windows;

namespace FriendStorage.UI.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();
            MainViewModel vm = container.Resolve<MainViewModel>();
            InitializeComponent();
            DataContext = vm;
        }
    }
}
