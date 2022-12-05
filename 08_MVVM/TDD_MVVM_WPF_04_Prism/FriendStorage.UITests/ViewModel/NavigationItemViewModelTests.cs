using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
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
        [Test]
        public void ShouldPublishOpenFriendEditViewEvent()
        {
            const int friendId = 7;
            var eventMock = new Mock<OpenFriendEditViewEvent>();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
                .Returns(eventMock.Object);
            var viewModel = new NavigationItemViewModel(friendId, "Przemek",
                eventAggregatorMock.Object);
            viewModel.OpenFriendEditViewCommand.Execute(null);
            eventMock.Verify(e => e.Publish(friendId), Times.Once);
        }
    }
}
