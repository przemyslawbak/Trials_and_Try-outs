using System;

namespace FriendStorage.UI.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
        public MainViewModel()
        {
            NavigationViewModel = new NavigationViewModel(); //inicjalizacja
        }
        public NavigationViewModel NavigationViewModel { get; private set; }

        public void Load()
        {
            NavigationViewModel.Load(); //łądujemy też nawigację przy załadowaniu widoku głównego
        }
    }
}
