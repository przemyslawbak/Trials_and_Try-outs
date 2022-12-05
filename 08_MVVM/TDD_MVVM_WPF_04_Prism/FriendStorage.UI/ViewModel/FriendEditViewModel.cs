using FriendStorage.Model;
using FriendStorage.UI.DataProvider;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int friendId);
        Friend Friend { get; } // 3 <- spr ID of friend
    }
    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private IFriendDataProvider _dataProvider; //ad 5
        private Friend _friend;

        public FriendEditViewModel(IFriendDataProvider dataProvider) //ad 5
        {
            _dataProvider = dataProvider; //ad 5
        }

        public Friend Friend
        {
            get
            {
                return _friend; //ad 5
            }
            set
            {
                _friend = value; // ad 5
                OnPropertyChanged(); //ad 6
            }
        }

        public void Load(int friendId) //ad 5
        {
            var friend = _dataProvider.GetFriendById(friendId);

            Friend = friend;
        }
    }
}
