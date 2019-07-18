using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        private NavigationViewModel _viewModel;
        private FriendSavedEvent _friendSavedEvent;
        private FriendDeletedEvent _friendDeletedEvent;

        public NavigationViewModelTests() //ctor for test methds
        {
            _friendSavedEvent = new FriendSavedEvent();
            _friendDeletedEvent = new FriendDeletedEvent();

            var eventAggregatorMock = new Mock<IEventAggregator>();
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
              .Returns(_friendSavedEvent);
            eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
              .Returns(_friendDeletedEvent);
            var navigationDataProviderMock = new Mock<INavigationDataProvider>(); //przy uzyciu Moq, zamiast klasy z końca pliku
            navigationDataProviderMock.Setup(dp => dp.GetAllFriends())
            .Returns(new List<LookupItem>
            {
                new LookupItem { Id = 1, DisplayMember = "Pszemek" },
                new LookupItem { Id = 2, DisplayMember = "Alicja" }
            });
            _viewModel = new NavigationViewModel(navigationDataProviderMock.Object, eventAggregatorMock.Object); //instancja makiety

        }
        [Fact]
        public void ShouldLoadFriends()
        {
            _viewModel.Load();

            Assert.Equal(2, _viewModel.Friends.Count);

            var friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 1);
            Assert.NotNull(friend);
            Assert.Equal("Pszemek", friend.DisplayMember);

            friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 2);
            Assert.NotNull(friend);
            Assert.Equal("Alicja", friend.DisplayMember);
        }

        [Fact]
        public void ShouldLoadFriendsOnlyOnce()
        {
            _viewModel.Load();
            _viewModel.Load();

            Assert.Equal(2, _viewModel.Friends.Count);
        }

        [Fact]
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

            Assert.Equal("Anna Huber", navigationItem.DisplayMember);
        }
        [Fact]
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

            Assert.Equal(3, _viewModel.Friends.Count);

            var addedItem = _viewModel.Friends.SingleOrDefault(f => f.Id == newFriendId);
            Assert.NotNull(addedItem);
            Assert.Equal("Anna Huber", addedItem.DisplayMember);
        }
        [Fact]
        public void ShouldRemoveNavigationItemWhenFriendIsDeleted()
        {
            _viewModel.Load();

            var deletedFriendId = _viewModel.Friends.First().Id;

            _friendDeletedEvent.Publish(deletedFriendId);

            Assert.Single(_viewModel.Friends);
            Assert.NotEqual(deletedFriendId, _viewModel.Friends.Single().Id);
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
