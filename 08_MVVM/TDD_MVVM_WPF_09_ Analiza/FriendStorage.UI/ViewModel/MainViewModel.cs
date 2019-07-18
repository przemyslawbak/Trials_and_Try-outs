using FriendStorage.DataAccess;
using FriendStorage.UI.Command;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

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
      eventAggregator.GetEvent<OpenFriendEditViewEvent>().Subscribe(OnOpenFriendEditView);
      eventAggregator.GetEvent<FriendDeletedEvent>().Subscribe(OnFriendDeleted);
      CloseFriendTabCommand = new DelegateCommand(OnCloseFriendTabExecute);
      AddFriendCommand = new DelegateCommand(OnAddFriendExecute);
    }

    private void OnFriendDeleted(int friendId)
    {
      var friendEditVm = FriendEditViewModels.Single(vm => vm.Friend.Id == friendId);
      FriendEditViewModels.Remove(friendEditVm);
    }

    private void OnCloseFriendTabExecute(object obj)
    {
      var friendEditVm = (IFriendEditViewModel)obj;
      FriendEditViewModels.Remove(friendEditVm);
    }

    private void OnAddFriendExecute(object obj)
    {
      SelectedFriendEditViewModel = CreateAndLoadFriendEditViewModel(null);
    }

    private IFriendEditViewModel CreateAndLoadFriendEditViewModel(int? friendId)
    {
      var friendEditVm = _friendEditVmCreator();
      FriendEditViewModels.Add(friendEditVm);
      friendEditVm.Load(friendId);
      return friendEditVm;
    }

    private void OnOpenFriendEditView(int friendId)
    {
      var friendEditVm = FriendEditViewModels.SingleOrDefault(vm => vm.Friend.Id == friendId);
      if (friendEditVm == null)
      {
        friendEditVm = CreateAndLoadFriendEditViewModel(friendId);
      }
      SelectedFriendEditViewModel = friendEditVm;
    }

    public ICommand AddFriendCommand { get; private set; }

    public ICommand CloseFriendTabCommand { get; private set; }

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
        OnPropertyChanged();
      }
    }

    public void Load()
    {
      NavigationViewModel.Load();
    }
  }
}
