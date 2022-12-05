using Wpf_Models;

namespace Wpf_Services.Controls
{
    public interface IControlsService
    {
        UiControlsModel SwitchOff();
        UiControlsModel SwitchOn();
    }
}