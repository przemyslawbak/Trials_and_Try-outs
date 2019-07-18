using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;
using NUnit.Framework;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendStorage.UITests.ViewModel
{
    [TestFixture]
    public class NavigationViewModelTests
    {
        private NavigationViewModel _viewModel;
        private FriendSavedEvent _friendSavedEvent;

        public NavigationViewModelTests() //ctor for test methds
        {
            _friendSavedEvent = new FriendSavedEvent();
            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
              .Returns(_friendSavedEvent);
            var navigationDataProviderMock = new Mock<INavigationDataProvider>(); //przy uzyciu Moq, zamiast klasy z końca pliku
            navigationDataProviderMock.Setup(dp => dp.GetAllFriends())
            .Returns(new List<LookupItem>
            {
                new LookupItem { Id = 1, DisplayMember = "Pszemek" },
                new LookupItem { Id = 2, DisplayMember = "Alicja" }
            });
            _viewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object); //instancja makiety

        }
        [Test]
        public void ShouldLoadFriends()
        {
            _viewModel.Load();

            Assert.AreEqual(2, _viewModel.Friends.Count);

            var friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 1);
            Assert.NotNull(friend);
            Assert.AreEqual("Pszemek", friend.DisplayMember);

            friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 2);
            Assert.NotNull(friend);
            Assert.AreEqual("Alicja", friend.DisplayMember);
        }

        [Test]
        public void ShouldLoadFriendsOnlyOnce()
        {
            _viewModel.Load();
            _viewModel.Load();

            Assert.AreEqual(2, _viewModel.Friends.Count);
        }

        [Test]
        public void ShouldUpdateNavigationItemWhenFriendIsSaved()
        {
            _viewModel.Load();
            var navigationItem = _viewModel.Friends.First();

            var friendId = navigationItem.Id;

            _friendSavedEvent.Publish(
              new Friend
              {
                  Id = friendId,
                  FirstName = "Anna",
                  LastName = "Huber"
              });

            Assert.AreEqual("Anna Huber", navigationItem.DisplayMember);
        }
        [Test]
        public void ShouldAddNavigationItemWhenAddedFriendIsSaved()
        {
            _viewModel.Load();

            const int newFriendId = 97;

            _friendSavedEvent.Publish(new Friend
            {
                Id = newFriendId,
                FirstName = "Anna",
                LastName = "Huber"
            });

            Assert.AreEqual(3, _viewModel.Friends.Count);

            var addedItem = _viewModel.Friends.SingleOrDefault(f => f.Id == newFriendId);
            Assert.NotNull(addedItem);
            Assert.AreEqual("Anna Huber", addedItem.DisplayMember);
        }
    }

    /*
    //bez wykorzystania Moq !!!!!!
    public class NavigationDataProviderMock : INavigationDataProvider //mock dla data provider
    {
        public IEnumerable<LookupItem> GetAllFriends()
        {
            yield return new LookupItem { Id = 1, DisplayMember = "Pszemek" };
            yield return new LookupItem { Id = 2, DisplayMember = "Alicja" };
        }
    }
    */
}
