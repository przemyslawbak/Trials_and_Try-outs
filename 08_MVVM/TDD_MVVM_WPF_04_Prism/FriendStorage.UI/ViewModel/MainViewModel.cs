using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace FriendStorage.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IFriendEditViewModel _selectedFriendEditViewModel;
        private Func<IFriendEditViewModel> _friendEditVmCreator;
        public MainViewModel(INavigationViewModel navigationViewModel,
          Func<IFriendEditViewModel> friendEditVmCreator,
          IEventAggregator eventAggregator)
        {
            NavigationViewModel = navigationViewModel;
            FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();
            _friendEditVmCreator = friendEditVmCreator;
            eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView); //ad 2
        }
        private void OnOpenFriendEditView(int friendId)
        {
            var friendEditVm = FriendEditViewModels.SingleOrDefault(vm => vm.Friend.Id == friendId); // ad 2 / 3
            if (friendEditVm == null) //if not found
            {
                friendEditVm = _friendEditVmCreator(); //ad 2
                FriendEditViewModels.Add(friendEditVm); // ad 2
                friendEditVm.Load(friendId); //ad 2
            }
            SelectedFriendEditViewModel = friendEditVm;
        }
        public INavigationViewModel NavigationViewModel { get; private set; }
        public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; private set; }
        public IFriendEditViewModel SelectedFriendEditViewModel
        {
            get
            {
                return _selectedFriendEditViewModel;
            }

            set
            {
                _selectedFriendEditViewModel = value;
                OnPropertyChanged(); // 4
            }
        }
        public void Load()
        {
            NavigationViewModel.Load();
        }
    }
}
