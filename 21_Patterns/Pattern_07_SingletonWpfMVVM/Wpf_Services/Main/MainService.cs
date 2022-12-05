using Autofac;
using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Files;

namespace Wpf_Services.Main
{
    public class MainService : IMainService
    {
        IFilesService _fileService;
        IControlsService _controlsService;
        IContainer _container;
        public MainService()
        {
            Initialize();
        }

        public void Initialize()
        {
            _container = Booter.BootStrap();

            var aggregator = _container.Resolve<Facade>();

            _fileService = aggregator.FilesService;
            _controlsService = aggregator.ControlsService;
        }

        public UiControlsModel SwitchOff()
        {
            return _controlsService.SwitchOff();
        }

        public UiControlsModel SwitchOn()
        {
            return _controlsService.SwitchOn();
        }
    }
}
