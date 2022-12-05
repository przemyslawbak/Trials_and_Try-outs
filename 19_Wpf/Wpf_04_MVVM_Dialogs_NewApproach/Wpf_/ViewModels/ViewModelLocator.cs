using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_.Startup;
using Wpf_.Views;
using Wpf_Services.Dialogs;

namespace Wpf_.ViewModels
{
    public class ViewModelLocator
    {
        IContainer _container;

        public ViewModelLocator()
        {
            _container = BootStrapper.BootStrap();

            IDialogService dialogService = new DialogService(MainWindow);
        }

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return _container.Resolve<MainWindowViewModel>();
            }
        }
    }
}
