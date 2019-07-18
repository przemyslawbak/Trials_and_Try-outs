using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendStorage.UITests.ViewModel
{
    [TestFixture]
    public class FriendEditViewModelTests
    {
        private const int _friendId = 5;
        private Mock<IFriendDataProvider> _dataProviderMock;
        private FriendEditViewModel _viewModel;

        public FriendEditViewModelTests()
        {
            _dataProviderMock = new Mock<IFriendDataProvider>();
            _dataProviderMock.Setup(dp => dp.GetFriendById(_friendId))
              .Returns(new Friend { Id = _friendId, FirstName = "Przemek" });

            _viewModel = new FriendEditViewModel(_dataProviderMock.Object);
        }
        //5 rozdział 6 plik 11
        [Test]
        public void ShouldLoadFriend()
        {
            _viewModel.Load(_friendId);

            Assert.NotNull(_viewModel.Friend);
            Assert.AreEqual(_friendId, _viewModel.Friend.Id);

            _dataProviderMock.Verify(dp => dp.GetFriendById(_friendId), Times.Once);
        }
        //6 rozdział 6 plik 11
        [Test]
        public void ShouldRaisePropertyChangedEventForFriend()
        {
            var fired = _viewModel.IsPropertyChangedFired(
              () => _viewModel.Load(_friendId),
              nameof(_viewModel.Friend));

            Assert.True(fired);
        }
    }
}
