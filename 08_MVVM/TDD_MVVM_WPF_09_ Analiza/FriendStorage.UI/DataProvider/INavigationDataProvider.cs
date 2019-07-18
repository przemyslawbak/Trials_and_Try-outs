using FriendStorage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendStorage.UI.DataProvider
{
    public interface INavigationDataProvider //jedna metoda z IDataService
    {
        IEnumerable<LookupItem> GetAllFriends();
    }
}
