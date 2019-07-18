using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Testy_04_MVVM_rozne_rozwiazania.Views;

namespace Testy_04_MVVM_rozne_rozwiazania.ViewModels
{
    class WindowDialog
    {
        public void ShowWindow(object viewModel)
        {
            var win = new Window();
            win.Content = viewModel;
            win.Show();
        }

    }


}

