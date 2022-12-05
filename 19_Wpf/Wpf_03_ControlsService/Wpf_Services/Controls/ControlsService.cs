using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_Models;

namespace Wpf_Services.Controls
{
    public class ControlsService : IControlsService
    {
        public UiControlsModel SwitchOff()
        {
            UiControlsModel uiModel = new UiControlsModel()
            {
                Jeden = false,
                Dwa = false,
                Trzy = false
            };

            return uiModel;
        }

        public UiControlsModel SwitchOn()
        {
            UiControlsModel uiModel = new UiControlsModel()
            {
                Jeden = true,
                Dwa = true,
                Trzy = true
            };

            return uiModel;
        }
    }
}
