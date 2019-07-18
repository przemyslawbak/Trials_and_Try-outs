using FriendStorage.Model;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using FriendStorage.UI.Wrapper;
using Prism.Events;
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

        public FriendEditViewModel(IFriendDataProvider dataProvider,
          IEventAggregator eventAggregator) //IEventAggregator dla aktualizacji nazwisk w liście nawigacji
        {
            _dataProvider = dataProvider;
            _eventAggregator = eventAggregator; //dla IEventAggregator
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute); //8
        }
        //8
        public ICommand SaveCommand { get; private set; }

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
            //ad 11
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }
        //ad 10
        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
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
