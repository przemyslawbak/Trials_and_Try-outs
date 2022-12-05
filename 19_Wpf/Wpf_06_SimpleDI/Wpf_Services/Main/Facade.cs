using Wpf_Services.Controls;
using Wpf_Services.Files;

namespace Wpf_Services.Main
{
    public class Facade
    {
        public Facade(IFilesService fileServ, IControlsService contrServ)
        {
            FilesService = fileServ;
            ControlsService = contrServ;
        }

        public IFilesService FilesService { get; set; }
        public IControlsService ControlsService { get; set; }
    }
}
