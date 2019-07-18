using System;
using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider
{
    /// <summary>
    /// Dostawca danych, korzysta z interfejsu w wartstwie DataAccess
    /// </summary>
    public class FriendDataProvider : IFriendDataProvider
    {
        private readonly Func<IDataService> _dataServiceCreator; //pole dla DataAccess

        public FriendDataProvider(Func<IDataService> dataServiceCreator) //ctor
        {
            _dataServiceCreator = dataServiceCreator; //wstrzyknięcie zależności
        }
        //implementacja interfejsu
        public Friend GetFriendById(int id)
        {
            using (var dataService = _dataServiceCreator())
            {
                return dataService.GetFriendById(id);
            }
        }

        public void SaveFriend(Friend friend)
        {
            using (var dataService = _dataServiceCreator())
            {
                dataService.SaveFriend(friend);
            }
        }

        public void DeleteFriend(int id)
        {
            using (var dataService = _dataServiceCreator())
            {
                dataService.DeleteFriend(id);
            }
        }
    }
}
