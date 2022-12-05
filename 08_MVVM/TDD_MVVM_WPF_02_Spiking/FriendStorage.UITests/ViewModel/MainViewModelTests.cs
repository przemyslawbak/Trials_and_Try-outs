using FriendStorage.UI.ViewModel;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace FriendStorage.UITests.ViewModel
{
    [TestFixture]
    public class MainViewModelTests
    {
        [Test]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            var navigationViewModelMock = new NavigationViewModelMock();
            var viewModel = new MainViewModel(navigationViewModelMock);

            viewModel.Load();

            Assert.True(navigationViewModelMock.LoadHasBeenCalled);
        }
    }
    public class NavigationViewModelMock :
    INavigationViewModel
    {
        public bool LoadHasBeenCalled { get; set; }
        public void Load()
        {
            LoadHasBeenCalled = true;
        }
    }
}
