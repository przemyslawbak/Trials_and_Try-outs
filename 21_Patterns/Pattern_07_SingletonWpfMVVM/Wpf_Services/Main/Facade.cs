using Wpf_Services.Controls;
using Wpf_Services.Files;

namespace Wpf_Services.Main
{
    public class Facade
    {
        public Facade(IFilesService filesService, IControlsService controlsService)
        {
            FilesService = filesService;
            ControlsService = controlsService;
        }

        public IFilesService FilesService { get; }

        public IControlsService ControlsService { get; }
    }
}
