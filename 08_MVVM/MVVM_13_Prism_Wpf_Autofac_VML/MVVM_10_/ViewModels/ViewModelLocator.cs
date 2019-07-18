using Autofac;
using MVVM_10_.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_10_.ViewModels
{
    public class ViewModelLocator
    {
        IContainer container = BootStrapper.BootStrap();

        public MainWindowViewModel MainWindowViewModel
        {
            get { return container.Resolve<MainWindowViewModel>(); }
        }

        public ChildView1ViewModel ChildView1ViewModel
        {
            get { return container.Resolve<ChildView1ViewModel>(); }
        }
    }
}
