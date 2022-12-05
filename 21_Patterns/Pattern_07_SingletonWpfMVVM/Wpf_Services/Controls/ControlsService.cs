using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_Models;
using Wpf_Services.Singleton;

namespace Wpf_Services.Controls
{
    public class ControlsService : IControlsService
    {
        ISingleton _singleton = Singleton.Singleton.Instance;

        public UiControlsModel SwitchOff()
        {
            UiControlsModel uiModel = new UiControlsModel()
            {
                Jeden = false,
                Dwa = false,
                Trzy = false
            };

            _singleton.ValueOne = 666;

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

            _singleton.ValueOne = 666;

            return uiModel;
        }
    }
}
