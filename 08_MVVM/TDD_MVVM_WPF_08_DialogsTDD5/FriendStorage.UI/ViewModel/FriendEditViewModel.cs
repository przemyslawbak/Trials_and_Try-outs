using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Dialogs;
using FriendStorage.UI.Events;
using FriendStorage.UI.Wrapper;
using Prism.Events;
using System.Windows;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int? friendId);
        FriendWrapper Friend { get; }
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider;
        private FriendWrapper _friend;
        private IEventAggregator _eventAggregator;
        private IMessageDialogService _messageDialogService;

        //dialogi, IEventAggregator dla aktualizacji nazwisk w liście nawigacji
        public FriendEditViewModel(IFriendDataProvider dataProvider,
      IEventAggregator eventAggregator,
      IMessageDialogService messageDialogService) //
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute); //8
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
        }
        //8
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public FriendWrapper Friend
        {
            get
            {
                return _friend;
            }
            set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public void Load(int? friendId)
        {
            var friend = friendId.HasValue
        ? _dataProvider.GetFriendById(friendId.Value)
        : new Friend(); //nowy friend

            Friend = new FriendWrapper(friend);
            //ad 10
            Friend.PropertyChanged += Friend_PropertyChanged;
            InvalidateCommands();
        }
        //ad 10
        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateCommands();
        }
        private void InvalidateCommands()
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();//ad 11
            ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();//ad delete
        }
        private void OnDeleteExecute(object obj)
        {
            var result = _messageDialogService.ShowYesNoDialog("Delete Friend",
              $"Do you really want to delete the friend '{Friend.FirstName} {Friend.LastName}'");
            if (result == MessageDialogResult.Yes)
            {
                _dataProvider.DeleteFriend(Friend.Id);
                _eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private bool OnDeleteCanExecute(object arg)
        {
            return Friend != null && Friend.Id > 0;
        }
        private void OnSaveExecute(object obj)
        {
            _dataProvider.SaveFriend(Friend.Model); //ad 12
            Friend.AcceptChanges(); //ad 13
            _eventAggregator.GetEvent<FriendSavedEvent>().Publish(Friend.Model);
        }
        //ad 8
        private bool OnSaveCanExecute(object arg)
        {
            return Friend != null && Friend.IsChanged;
        }
    }
}
