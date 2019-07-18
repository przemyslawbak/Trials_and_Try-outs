using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendStorage.UITests.ViewModel
{
    [TestFixture]
    public class NavigationViewModelTests
    {
        [Test]
        public void ShouldLoadFriends()
        {
            var viewModel = new NavigationViewModel(new NavigationDataProviderMock()); //instancja makiety
            viewModel.Load();
            Assert.AreEqual(2, viewModel.Friends.Count);
        }
    }
    public class NavigationDataProviderMock : INavigationDataProvider //mock dla data provider
    {
        public IEnumerable<Friend> GetAllFriends()
        {
            yield return new Friend { Id = 1, FirstName = "Pszemek" };
            yield return new Friend { Id = 2, FirstName = "Alicja" };
        }
    }
}
