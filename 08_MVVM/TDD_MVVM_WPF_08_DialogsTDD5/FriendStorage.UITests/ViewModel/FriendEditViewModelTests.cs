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
    public class FriendEditViewModelTests
    {
        private const int _friendId = 5;
        private Mock<IFriendDataProvider> _dataProviderMock;
        private FriendEditViewModel _viewModel;
        private Mock<FriendSavedEvent> _friendSavedEventMock;
        private Mock<IEventAggregator> _eventAggregatorMock;
        private Mock<FriendDeletedEvent> _friendDeletedEventMock;
        private Mock<IMessageDialogService> _messageDialogServiceMock;

        public FriendEditViewModelTests()
        {
            _friendDeletedEventMock = new Mock<FriendDeletedEvent>();
            _friendSavedEventMock = new Mock<FriendSavedEvent>();

            _eventAggregatorMock = new Mock<IEventAggregator>();
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendSavedEvent>())
              .Returns(_friendSavedEventMock.Object);
            _eventAggregatorMock.Setup(ea => ea.GetEvent<FriendDeletedEvent>())
              .Returns(_friendDeletedEventMock.Object);

            _dataProviderMock = new Mock<IFriendDataProvider>();
            _dataProviderMock.Setup(dp => dp.GetFriendById(_friendId))
              .Returns(new Friend { Id = _friendId, FirstName = "Pszemek" });

            _messageDialogServiceMock = new Mock<IMessageDialogService>();

            _viewModel = new FriendEditViewModel(_dataProviderMock.Object,
              _eventAggregatorMock.Object,
              _messageDialogServiceMock.Object);
        }
        //5

        //rozdział 6 plik 11
        //od rozdziału 7 idzie tylko w DEBUG
        [Fact]
        public void ShouldLoadFriend()
        {
            _viewModel.Load(_friendId);

            Assert.NotNull(_viewModel.Friend);
            Assert.Equal(_friendId, _viewModel.Friend.Id);

            _dataProviderMock.Verify(dp => dp.GetFriendById(_friendId), Times.Once);
        }

        //6
        //rozdział 6 plik 11
        [Fact]
        public void ShouldRaisePropertyChangedEventForFriend()
        {
            var fired = _viewModel.IsPropertyChangedFired(
              () => _viewModel.Load(_friendId),
              nameof(_viewModel.Friend));
            Assert.True(fired);
        }
        //8
        [Fact]
        public void ShouldDisableSaveCommandWhenFriendIsLoaded()
        {
            _viewModel.Load(_friendId);
            Assert.False(_viewModel.SaveCommand.CanExecute(null));
        }
        //9
        [Fact]
        public void ShouldEnableSaveCommandWhenFriendIsChanged()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";
            Assert.True(_viewModel.SaveCommand.CanExecute(null));
        }
        //10
        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandWhenFriendIsChanged()
        {
            _viewModel.Load(_friendId);
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Friend.FirstName = "Changed";
            Assert.True(fired);
        }
        //11
        [Fact]
        public void ShouldRaiseCanExecuteChangedForSaveCommandAfterLoad()
        {
            var fired = false;
            _viewModel.SaveCommand.CanExecuteChanged += (s, e) => fired = true;
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
        public void ShouldRaiseCanExecuteChangedForDeleteCommandAfterLoad()
        {
            var fired = false;
            _viewModel.DeleteCommand.CanExecuteChanged += (s, e) => fired = true;
            _viewModel.Load(_friendId);
            Assert.True(fired);
        }
        //12
        [Fact]
        public void ShouldCallSaveMethodOfDataProviderWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";
            _viewModel.SaveCommand.Execute(null);
            _dataProviderMock.Verify(dp => dp.SaveFriend(_viewModel.Friend.Model), Times.Once);
        }
        //13
        [Fact]
        public void ShouldAcceptChangesWhenSaveCommandIsExecuted()
        {
            _viewModel.Load(_friendId);
            _viewModel.Friend.FirstName = "Changed";
            _viewModel.SaveCommand.Execute(null);
            Assert.False(_viewModel.Friend.IsChanged);
        }
        //14
        //dla publikacji w oknie nawigacji
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
