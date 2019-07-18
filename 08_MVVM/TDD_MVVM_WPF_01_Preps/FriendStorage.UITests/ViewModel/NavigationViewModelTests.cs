using FriendStorage.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class NavigationViewModelTests
    {
        [Fact]
        public void ShouldLoadFriends()
        {
            var viewModel = new NavigationViewModel();
            viewModel.Load();
            //Assert.Equal(2, viewModel.Friends.Count);
        }
    }
}
