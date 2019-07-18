using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
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

        public NavigationViewModelTests() //ctor for test methds
        {
            var eventAggregatorMock = new Mock<IEventAggregator>();
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
            _viewModel.Load(); //wczytanie przyjaciół
            Assert.AreEqual(2, _viewModel.Friends.Count);

            var friend = _viewModel.Friends.SingleOrDefault(f => f.Id == 1);
            Assert.AreEqual("Pszemek", friend.DisplayMember);
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
