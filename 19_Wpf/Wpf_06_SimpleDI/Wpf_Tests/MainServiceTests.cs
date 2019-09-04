using Moq;
using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Files;
using Wpf_Services.Main;
using Xunit;

namespace Wpf_Tests
{
    public class MainServiceTests
    {
        private readonly Mock<IControlsService> _controlsServiceMock;
        private readonly Mock<IFilesService> _filesServiceMock;
        private readonly MainService _vm;

        private readonly UiControlsModel _controlsModelOff = new UiControlsModel() { Dwa = true, Jeden = true, Trzy = true };
        private readonly UiControlsModel _controlsModelOn = new UiControlsModel() { Dwa = false, Jeden = false, Trzy = false };

        public MainServiceTests()
        {
            _controlsServiceMock = new Mock<IControlsService>();
            _filesServiceMock = new Mock<IFilesService>();

            _controlsServiceMock.Setup(c => c.SwitchOff())
                .Returns(_controlsModelOff);

            _controlsServiceMock.Setup(c => c.SwitchOn())
                .Returns(_controlsModelOn);

            _vm = new MainService();
            _vm.Facade = new Facade(_filesServiceMock.Object, _controlsServiceMock.Object);
        }

        [Fact]
        public void SwitchOff_Called_ReturnsOffModel()
        {
            _vm.Controls = new UiControlsModel() { Jeden = false, Dwa = false, Trzy = false };

            _vm.SwitchOff();

            Assert.True(_vm.Controls.Trzy);
        }

        [Fact]
        public void SwitchOn_Called_ReturnsOnModel()
        {
            _vm.Controls = new UiControlsModel() { Jeden = true, Dwa = true, Trzy = true };

            _vm.SwitchOn();

            Assert.False(_vm.Controls.Trzy);
        }
    }
}
