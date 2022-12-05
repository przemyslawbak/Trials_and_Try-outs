using Wpf_Models;

namespace Wpf_Services.Main
{
    public interface IMainService
    {
        void Initialize();
        UiControlsModel SwitchOff();
        UiControlsModel SwitchOn();
    }
}