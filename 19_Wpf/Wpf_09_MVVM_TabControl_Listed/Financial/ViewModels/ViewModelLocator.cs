using Autofac;

namespace Financial.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IContainer _container;

        public ViewModelLocator()
        {
            _container = Startup.BootStrap();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return _container.Resolve<MainViewModel>();
            }
        }

        public OtherViewModel OtherViewModel
        {
            get
            {
                return _container.Resolve<OtherViewModel>();
            }
        }

        public InputViewModel InputViewModel
        {
            get
            {
                return _container.Resolve<InputViewModel>();
            }
        }
    }
}
