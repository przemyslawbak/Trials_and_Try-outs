using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using System;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel)
        {
            NavigationViewModel = navigationViewModel;

        }
        public INavigationViewModel NavigationViewModel { get; private set; }

        public void Load()
        {
            NavigationViewModel.Load(); //spr przez test
        }
    }
}
