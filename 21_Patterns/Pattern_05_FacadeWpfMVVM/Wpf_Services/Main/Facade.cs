using Wpf_Services.Controls;
using Wpf_Services.Files;

namespace Wpf_Services.Main
{
    public class Facade
    {
        IFilesService _fileService;
        IControlsService _controlsService;
        public Facade(IFilesService filesService, IControlsService controlsService)
        {
            _fileService = filesService;
            _controlsService = controlsService;

            FilesService = _fileService;
            ControlsService = _controlsService;
        }

        public IFilesService FilesService { get; }

        public IControlsService ControlsService { get; }
    }
}
