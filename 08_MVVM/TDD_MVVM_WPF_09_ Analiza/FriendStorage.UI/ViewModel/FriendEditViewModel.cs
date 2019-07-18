using FriendStorage.Model;
using System;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Command;
using System.Windows.Input;
using FriendStorage.UI.Wrapper;
using Prism.Events;
using FriendStorage.UI.Events;
using System.Windows;
using FriendStorage.UI.Dialogs;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel //interfejs wczytania przyjaciela
    {
        void Load(int? friendId); //metoda wczytania z nullable parametrem ID
        FriendWrapper Friend { get; } //
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel //VM dla wyświetlenia i edycji przyjaciela
    {
        private IFriendDataProvider _dataProvider; //pole dla dostawcy danych
        private FriendWrapper _friend; //wrapper
        private IEventAggregator _eventAggregator; //Prism
        private IMessageDialogService _messageDialogService; //dialogi

        public FriendEditViewModel(IFriendDataProvider dataProvider, IEventAggregator eventAggregator,
          IMessageDialogService messageDialogService) //ctor
        {
            //wstrzykiwane zależności
            _dataProvider = dataProvider; //data
            _eventAggregator = eventAggregator; //Prism
            _messageDialogService = messageDialogService; //dialogs
            //??
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute, OnDeleteCanExecute);
        }

        public ICommand SaveCommand { get; private set; }

        public ICommand DeleteCommand { get; private set; }

        //prop dla przyjaciela
        public FriendWrapper Friend
        {
            get
            {
                return _friend; //zwraca fumfla z wrappera
            }
            set
            {
                _friend = value; //ustawia fumwla przez wrappera
                OnPropertyChanged();
            }
        }

        public void Load(int? friendId) //implementacja interfejsu wczytania frienda
        {
            var friend = friendId.HasValue
              ? _dataProvider.GetFriendById(friendId.Value)
              : new Friend(); //jak jest ID, to pobieranie z pliku, jak nie to tworzymy nowego

            Friend = new FriendWrapper(friend); //tworzenie obiektu

            Friend.PropertyChanged += Friend_PropertyChanged; //jeśli się zmieni to +=

            InvalidateCommands(); //spr czy można wykonać komendy
        }

        private void Friend_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvalidateCommands(); //spr czy można wykonać komendy
        }

        private void InvalidateCommands() //metoda dla spr czy można wykonać komendy przechodzą przez DelegateCommand
        {
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            ((DelegateCommand)DeleteCommand).RaiseCanExecuteChanged();
        }

        private void OnSaveExecute(object obj) //metoda zapisu
        {
            _dataProvider.SaveFriend(Friend.Model);
            Friend.AcceptChanges();
            _eventAggregator.GetEvent<FriendSavedEvent>().Publish(Friend.Model);
        }

        private bool OnSaveCanExecute(object arg) //metoda spr czy może zapisać
        {
            return Friend != null && Friend.IsChanged;
        }

        private void OnDeleteExecute(object obj) //metoda usuwania + dialog
        {
            var result = _messageDialogService.ShowYesNoDialog("Delete Friend",
              $"Do you really want to delete the friend '{Friend.FirstName} {Friend.LastName}'");
            if (result == MessageDialogResult.Yes)
            {
                _dataProvider.DeleteFriend(Friend.Id);
                _eventAggregator.GetEvent<FriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private bool OnDeleteCanExecute(object arg) //metoda spr czy może usunąć
        {
            return Friend != null && Friend.Id > 0;
        }
    }
}
