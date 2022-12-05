using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Files;

namespace Wpf_Services.Main
{
    public class MainService : IMainService
    {
        private IControlsService _controlsService;

        public UiControlsModel Controls { get; set; }
        public Facade Facade { get; set; }

        public MainService()
        {
            Initialize();
        }

        public void Initialize()
        {
            Facade = new Facade(new FilesService(), new ControlsService());
        }

        public UiControlsModel SwitchOff()
        {
            _controlsService = Facade.ControlsService;
            Controls = _controlsService.SwitchOff();
            return Controls;
        }

        public UiControlsModel SwitchOn()
        {
            _controlsService = Facade.ControlsService;
            Controls = _controlsService.SwitchOn();
            return Controls;
        }
    }
}
