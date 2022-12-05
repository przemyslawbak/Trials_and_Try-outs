using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_.Startup;

namespace Wpf_.ViewModels
{
    public class ViewModelLocator
    {
        IContainer container = BootStrapper.BootStrap();

        public MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return container.Resolve<MainWindowViewModel>();
            }
        }
    }
}
