using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WpfApplication1.Startup;
using Autofac;
using WpfApplication1.Views;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    //credits: https://www.c-sharpcorner.com/article/dialogs-in-wpf-mvvm-part-three/
    //SO przyklad https://stackoverflow.com/questions/13840283/how-to-use-autofac-to-inject-viewmodels-in-a-windows-phone-8-application
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();
            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }
    }
}
