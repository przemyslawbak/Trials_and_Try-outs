using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider
{
  public interface IFriendDataProvider // trzy metody z IDataService
    {
    Friend GetFriendById(int id);

    void SaveFriend(Friend friend);

    void DeleteFriend(int id);
  }
}