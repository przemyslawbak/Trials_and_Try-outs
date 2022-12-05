using Moq;
using Wpf_Models;
using Wpf_Services.Object;
using Xunit;

namespace Wpf_Tests
{
    public class ObjectServiceTests
    {
        private readonly ObjectService _serv;
        private readonly Mock<IObjectFactory> _factoryMock;
        private readonly Mock<ISomeService> _someServMock;
        private readonly string _someFirstName = "SomeFirstName";

        public ObjectServiceTests()
        {
            _someServMock = new Mock<ISomeService>();
            _someServMock.SetupGet(s => s.SecondName).Returns("Cos");

            _factoryMock = new Mock<IObjectFactory>();
            _factoryMock.Setup(f => f.GetObject(_someFirstName))
                .Returns(new ObjectModel(_someFirstName, _someServMock.Object));

            _serv = new ObjectService();
        }

        [Fact]
        public void GetObject_Called_ReturnsObject()
        {
            _serv.Factory = _factoryMock.Object;

            IObjectModel newObj = _serv.GetObject(_someFirstName);

            Assert.Equal(_someFirstName + ", Cos", newObj.FullName);
        }
    }
}
