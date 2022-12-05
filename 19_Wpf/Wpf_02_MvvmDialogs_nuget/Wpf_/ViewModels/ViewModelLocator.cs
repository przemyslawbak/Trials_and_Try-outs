using Autofac;
using Wpf_.Startup;

namespace Wpf_.ViewModels
{
    public class ViewModelLocator
    {
        IContainer container = BootStrapper.BootStrap();

        public MainViewModel MainViewModel
        {
            get
            {
                return container.Resolve<MainViewModel>();
            }
        }

        public AddTextDialogViewModel AddTextDialogViewModel
        {
            get
            {
                return container.Resolve<AddTextDialogViewModel>();
            }
        }
    }
}
