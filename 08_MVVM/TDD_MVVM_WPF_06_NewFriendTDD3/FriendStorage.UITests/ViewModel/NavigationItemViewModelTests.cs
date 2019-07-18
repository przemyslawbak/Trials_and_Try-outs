using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;
using NUnit.Framework;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendStorage.UITests.ViewModel
{
    [TestFixture]
    public class NavigationItemViewModelTests
    {
        const int _friendId = 7;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private NavigationItemViewModel _viewModel;

        public NavigationItemViewModelTests()
        {
            _eventAggregatorMock = new Mock<IEventAggregator>();

            _viewModel = new NavigationItemViewModel(_friendId,
              "Thomas", _eventAggregatorMock.Object);
        }
        [Test]
        public void ShouldPublishOpenFriendEditViewEvent()
        {
            var eventMock = new Mock<OpenFriendEditViewEvent>();
            _eventAggregatorMock
              .Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
              .Returns(eventMock.Object);

            _viewModel.OpenFriendEditViewCommand.Execute(null);

            eventMock.Verify(e => e.Publish(_friendId), Times.Once);
        }
        [Test]
        public void ShouldRaisePropertyChangedEventForDisplayMember()
        {
            var fired = _viewModel.IsPropertyChangedFired(() =>
            {
                _viewModel.DisplayMember = "Changed";
            }, nameof(_viewModel.DisplayMember));

            Assert.True(fired);
        }
    }
}
