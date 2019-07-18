using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.Startup;

namespace WpfApplication1.ViewModels
{
    public class ViewModelLocator
    {
        private MainWindowViewModel mainWindowViewModel = null;
        public MainWindowViewModel MainWindowViewModel
        {
            get { return mainWindowViewModel; }
            set { mainWindowViewModel = value; }
        }

        public ViewModelLocator()
        {
            var bootStrapper = new BootStrapper();
            var container = bootStrapper.BootStrap();
            this.mainWindowViewModel = container.Resolve<MainWindowViewModel>();
        }
    }
}
