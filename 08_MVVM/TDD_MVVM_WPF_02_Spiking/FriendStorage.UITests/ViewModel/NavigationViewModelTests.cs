using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            viewModel.Load(); //wczytanie przyjaciół
            Assert.AreEqual(2, viewModel.Friends.Count);

            var friend = viewModel.Friends.SingleOrDefault(f => f.Id == 1);
            Assert.AreEqual("Pszemek", friend.DisplayMember);
        }
    }
    //bez wykorzystania Moq !!!!!!
    public class NavigationDataProviderMock : INavigationDataProvider //mock dla data provider
    {
        public IEnumerable<LookupItem> GetAllFriends()
        {
            yield return new LookupItem { Id = 1, DisplayMember = "Pszemek" };
            yield return new LookupItem { Id = 2, DisplayMember = "Alicja" };
        }
    }
}
