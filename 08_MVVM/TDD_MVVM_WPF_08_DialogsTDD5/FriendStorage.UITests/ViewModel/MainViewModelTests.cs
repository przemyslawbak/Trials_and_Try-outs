using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UI.Wrapper;
using FriendStorage.UITests.Extensions;
using Moq;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    public class MainViewModelTests
    {
        private Mock<INavigationViewModel> _navigationViewModelMock;
        private MainViewModel _viewModel;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private OpenFriendEditViewEvent _openFriendEditViewEvent;
        private List<Mock<IFriendEditViewModel>> _friendEditViewModelMocks;
        private FriendDeletedEvent _friendDeletedEvent;

        public MainViewModelTests()
        {
            _friendDeletedEvent = new FriendDeletedEvent();
            _friendEditViewModelMocks = new List<Mock<IFriendEditViewModel>>();
            _navigationViewModelMock = new Mock<INavigationViewModel>();
            _openFriendEditViewEvent = new OpenFriendEditViewEvent();
            _eventAggregatorMock = new Mock<IEventAggregator>();
            _eventAggregatorMock.Setup(ea => ea.GetEvent<OpenFriendEditViewEvent>())
              .Returns(_openFriendEditViewEvent);
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
              .Returns(_friendDeletedEvent);
            _viewModel = new MainViewModel(_navigationViewModelMock.Object,
              CreateFriendEditViewModel, _eventAggregatorMock.Object);
        }

        private IFriendEditViewModel CreateFriendEditViewModel()
        {
            var friendEditViewModelMock = new Mock<IFriendEditViewModel>();
            friendEditViewModelMock.Setup(vm => vm.Load(It.IsAny<int>()))
              .Callback<int?>(friendId =>
              {
                  friendEditViewModelMock.Setup(vm => vm.Friend)
            .Returns(new FriendWrapper(new Friend { Id = friendId.Value }));
              });
            _friendEditViewModelMocks.Add(friendEditViewModelMock);
            return friendEditViewModelMock.Object;
        }
        //1
        [Fact]
        public void ShouldCallTheLoadMethodOfTheNavigationViewModel()
        {
            _viewModel.Load();

            _navigationViewModelMock.Verify(vm => vm.Load(), Times.Once);
        }
        //2
        [Fact]
        //rozdzial 6 film 8 4:34 <- pierwszy TDD
        public void ShouldAddFriendEditViewModelAndLoadAndSelectIt()
        {
            const int friendId = 7;
            _openFriendEditViewEvent.Publish(friendId);

            Assert.Single(_viewModel.FriendEditViewModels);
            var friendEditVm = _viewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVm, _viewModel.SelectedFriendEditViewModel);
            _friendEditViewModelMocks.First().Verify(vm => vm.Load(friendId), Times.Once);
        }
        //3
        //rozdzial 6 film 9 0:00
        [Fact]
        public void ShouldAddFriendEditViewModelsOnlyOnce()
        {
            _openFriendEditViewEvent.Publish(5);
            _openFriendEditViewEvent.Publish(5);
            _openFriendEditViewEvent.Publish(6);
            _openFriendEditViewEvent.Publish(7);
            _openFriendEditViewEvent.Publish(7);

            Assert.Equal(3, _viewModel.FriendEditViewModels.Count);
        }
        //4
        //rozdział 6 film 10 0:00
        [Fact]
        public void ShouldRaisePropertyChangedEventForSelectedFriendEditViewModel()
        {
            var friendEditVmMock = new Mock<IFriendEditViewModel>();
            var fired = _viewModel.IsPropertyChangedFired(() =>
            {
                _viewModel.SelectedFriendEditViewModel = friendEditVmMock.Object;
            }, nameof(_viewModel.SelectedFriendEditViewModel));

            Assert.True(fired);
        }
        //7

        //rozdział 7 film 2
        //PRZECHODZI Z DEBUG
        [Fact]
        public void ShouldRemoveFriendEditViewModelOnCloseFriendTabCommand()
        {
            _openFriendEditViewEvent.Publish(7);

            var friendEditVm = _viewModel.SelectedFriendEditViewModel;

            _viewModel.CloseFriendTabCommand.Execute(friendEditVm);

            Assert.Empty(_viewModel.FriendEditViewModels);
        }

        [Fact]
        //R8a
        //rozdzial 6 film 8 4:34 <- pierwszy TDD
        public void ShouldAddFriendEditViewModelAndLoadItWithIdNullAndSelectIt()
        {
            _viewModel.AddFriendCommand.Execute(null);

            Assert.Single(_viewModel.FriendEditViewModels);
            var friendEditVm = _viewModel.FriendEditViewModels.First();
            Assert.Equal(friendEditVm, _viewModel.SelectedFriendEditViewModel);
            _friendEditViewModelMocks.First().Verify(vm => vm.Load(null), Times.Once);
        }
        [Fact]
        public void ShouldRemoveFriendEditViewModelOnFriendDeletedEvent()
        {
            const int deletedFriendId = 7;

            _openFriendEditViewEvent.Publish(deletedFriendId);
            _openFriendEditViewEvent.Publish(8);
            _openFriendEditViewEvent.Publish(9);

            _friendDeletedEvent.Publish(deletedFriendId);

            Assert.Equal(2, _viewModel.FriendEditViewModels.Count);
            Assert.True(_viewModel.FriendEditViewModels.All(vm => vm.Friend.Id != deletedFriendId));
        }
    }
    /*
    //Mock przed Moq
    public class NavigationViewModelMock :
    INavigationViewModel
    {
        public bool LoadHasBeenCalled { get; set; }
        public void Load()
        {
            LoadHasBeenCalled = true;
        }
    }
    */
}
