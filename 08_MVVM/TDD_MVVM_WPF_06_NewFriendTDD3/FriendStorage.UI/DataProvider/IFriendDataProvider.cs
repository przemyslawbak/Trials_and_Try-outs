using FriendStorage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendStorage.UI.DataProvider
{
    public interface IFriendDataProvider
    {
        Friend GetFriendById(int id);

        void SaveFriend(Friend friend);

        void DeleteFriend(int id);
    }
}
