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
        Mock<ISecondNameProvider> _secondNameProviderMock;
        Mock<IControlsService> _csMock;
        Mock<IObjectService> _osMock;
        MainWindowViewModel _vm;

        public MainWindowViewModelTests()
        {
            _secondNameProviderMock = new Mock<ISecondNameProvider>();
            _csMock = new Mock<IControlsService>();
            _osMock = new Mock<IObjectService>();

            _secondNameProviderMock.SetupGet(s => s.SecondName).Returns("Szpak");
            _osMock.Setup(o => o.GetObject(It.IsAny<string>())).Returns(new ObjectModel("Pszemek", _secondNameProviderMock.Object));

            _vm = new MainWindowViewModel(_csMock.Object, _osMock.Object);
        }

        [Fact]
        public void CreateNewObject_Called_UpdatesNewObject()
        {
            _vm.CreateNewObject("Pszemek");

            Assert.Equal("Pszemek", _vm.NewObject.FirstName);
            Assert.Equal("Szpak", _vm.NewObject.SecondName);
        }
    }
}
