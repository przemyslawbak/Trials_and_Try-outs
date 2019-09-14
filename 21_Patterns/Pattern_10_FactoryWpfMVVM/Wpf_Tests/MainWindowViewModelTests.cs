using Moq;
using Wpf_.ViewModels;
using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Object;
using Xunit;

namespace Wpf_Tests
{
    public class MainWindowViewModelTests
    {
        Mock<IControlsService> _csMock;
        Mock<IObjectService> _osMock;
        Mock<ISomeService> _ssMock;
        MainWindowViewModel _vm;

        public MainWindowViewModelTests()
        {
            _csMock = new Mock<IControlsService>();
            _osMock = new Mock<IObjectService>();
            _ssMock = new Mock<ISomeService>();

            _osMock.Setup(o => o.GetObject("Pszemek"))
                .Returns(new ObjectModel(It.IsAny<string>(), _ssMock.Object) { FullName = "hhh" });

            _vm = new MainWindowViewModel(_csMock.Object, _osMock.Object);
        }

        [Fact]
        public void CreateNewObject_Called_UpdatesNewObject()
        {
            _vm.CreateNewObject("Pszemek");

            Assert.Equal("hhh", _vm.NewObject.FullName);
        }
    }
}
