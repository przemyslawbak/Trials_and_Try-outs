using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
    /// <summary>
    /// testy dla widoku edycja fumfla
    /// </summary>
    public class FriendEditViewModelTests
    {
        private const int _friendId = 5; //testowane ID
        private FriendEditViewModel _viewModel; //VM
        //mocki
        private Mock<IFriendDataProvider> _dataProviderMock;
        private Mock<FriendSavedEvent> _friendSavedEventMock;
        private Mock<IEventAggregator> _eventAggregatorMock; //Prism
        private Mock<FriendDeletedEvent> _friendDeletedEventMock;
        private Mock<IMessageDialogService> _messageDialogServiceMock;

        public FriendEditViewModelTests()
        {
            _friendDeletedEventMock = new Mock<FriendDeletedEvent>(); //inst
            _friendSavedEventMock = new Mock<FriendSavedEvent>(); //inst

            _eventAggregatorMock = new Mock<IEventAggregator>(); //inst / Prism
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
              .Returns(_friendSavedEventMock.Object); //typ zwracanej wartości
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
              .Returns(_friendDeletedEventMock.Object); //typ zwracanej wartości

            _dataProviderMock = new Mock<IFriendDataProvider>(); //inst
            _dataProviderMock.Setup(dp => dp.GetFriendById(_friendId))
              .Returns(new Friend { Id = _friendId, FirstName = "Thomas" }); //typ zwracanej wartości

            _messageDialogServiceMock = new Mock<IMessageDialogService>(); //inst

            _viewModel = new FriendEditViewModel(_dataProviderMock.Object,
              _eventAggregatorMock.Object,
              _messageDialogServiceMock.Object); //inst
        }

        [Fact]
        public void ShouldLoadFriend()
        {
            _viewModel.Load(_friendId);

            Assert.NotNull(_viewModel.Friend);
            Assert.Equal(_friendId, _viewModel.Friend.Id);

            _dataProviderMock.Verify(dp => dp.GetFriendById(_friendId), Times.Once);
        }

        [Fact]
        public void ShouldRaisePropertyChangedEventForFriend()
        {
            var fired = _viewModel.IsPropertyChangedFired(
              () => _viewModel.Load(_friendId),
              nameof(_viewModel.Friend));

            Assert.True(fired);
        }

        [Fact]
        public void ShouldDisableSaveCommandWhenFriendIsLoaded()
        {
            _viewModel.Load(_friendId);

            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldEnableSaveCommandWhenFriendIsChanged()
        {
            _viewModel.Load(_friendId);

            _viewModel.Friend.FirstName = "Changed";

            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableSaveCommandWithoutLoad()
        {
            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandWhenFriendIsChanged()
        {
            _viewModel.Load(_friendId);
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Friend.FirstName = "Changed";
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
        {
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Load(_friendId);
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad()
        {
            var fired = false;
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Load(_friendId);
            Assert.True(fired);
        }

        [Fact]
        public void ShouldRaiseCanExecuteChangedForDeleteCommandWhenAcceptingChanges()
        {
            _viewModel.Load(_friendId);
            var fired = false;
            _viewModel.Friend.FirstName = "Changed";
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Friend.AcceptChanges();
            Assert.True(fired);
        }

        [Fact]
        public void ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            _viewModel.SaveCommand.Execute(null);
            _dataProviderMock.Verify(dp => dp.SaveFriend(_viewModel.Friend.Model), Times.Once);
        }

        [Fact]
        public void ShouldAcceptChangesWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            _viewModel.SaveCommand.Execute(null);
            Assert.False(_viewModel.Friend.IsChanged);
        }

        [Fact]
        public void ShouldPublishFriendSavedEventWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";

            _viewModel.SaveCommand.Execute(null);
            _friendSavedEventMock.Verify(e => e.Publish(_viewModel.Friend.Model), Times.Once);
        }

        [Fact]
        public void ShouldCreateNewFriendWhenNullIsPassedToLoadMethod()
        {
            _viewModel.Load(null);

            Assert.NotNull(_viewModel.Friend);
            Assert.Equal(0, _viewModel.Friend.Id);
            Assert.Null(_viewModel.Friend.FirstName);
            Assert.Null(_viewModel.Friend.LastName);
            Assert.Null(_viewModel.Friend.Birthday);
            Assert.False(_viewModel.Friend.IsDeveloper);

            _dataProviderMock.Verify(dp => dp.GetFriendById(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public void ShouldEnableDeleteCommandForExistingFriend()
        {
            _viewModel.Load(_friendId);
            Assert.True(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableDeleteCommandForNewFriend()
        {
            _viewModel.Load(null);
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableDeleteCommandWithoutLoad()
        {
            Assert.False(_viewModel.DeleteCommand.CanExecute(null));
        }

        [Theory]
        [InlineData(MessageDialogResult.Yes, 1)]
        [InlineData(MessageDialogResult.No, 0)]
        public void ShouldCallDeleteFriendWhenDeleteCommandIsExecuted(
          MessageDialogResult result, int expectedDeleteFriendCalls)
        {
            _viewModel.Load(_friendId);

            _messageDialogServiceMock.Setup(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
              It.IsAny<string>())).Returns(result);

            _viewModel.DeleteCommand.Execute(null);

            _dataProviderMock.Verify(dp => dp.DeleteFriend(_friendId),
              Times.Exactly(expectedDeleteFriendCalls));
            _messageDialogServiceMock.Verify(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
              It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(MessageDialogResult.Yes, 1)]
        [InlineData(MessageDialogResult.No, 0)]
        public void ShouldPublishFriendDeletedEventWhenDeleteCommandIsExecuted(
          MessageDialogResult result, int expectedPublishCalls)
        {
            _viewModel.Load(_friendId);

            _messageDialogServiceMock.Setup(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
             It.IsAny<string>())).Returns(result);

            _viewModel.DeleteCommand.Execute(null);

            _friendDeletedEventMock.Verify(e => e.Publish(_friendId),
              Times.Exactly(expectedPublishCalls));

            _messageDialogServiceMock.Verify(ds => ds.ShowYesNoDialog(It.IsAny<string>(),
             It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void ShouldDisplayCorrectMessageInDeleteDialog()
        {
            _viewModel.Load(_friendId);

            var f = _viewModel.Friend;
            f.FirstName = "Thomas";
            f.LastName = "Huber";

            _viewModel.DeleteCommand.Execute(null);

            _messageDialogServiceMock.Verify(d => d.ShowYesNoDialog("Delete Friend",
              $"Do you really want to delete the friend '{f.FirstName} {f.LastName}'"),
              Times.Once);
        }
    }
}
