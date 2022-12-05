using FriendStorage.Model;
using FriendStorage.UI.ViewModel;
using System;
using System.Runtime.CompilerServices;

namespace FriendStorage.UI.Wrapper
{
    /// <summary>
    /// propy przyjaciela + prop dla zmian w widoku FriendEdit
    /// </summary>
  public class FriendWrapper : ViewModelBase
  {
    private Friend _friend;
    private bool _isChanged;

    public FriendWrapper(Friend friend)
    {
      _friend = friend;
    }

    public Friend Model { get { return _friend; } }

    public bool IsChanged
    {
      get { return _isChanged; }
      private set
      {
        _isChanged = value;
        OnPropertyChanged();
      }
    }

    public void AcceptChanges()
    {
      IsChanged = false;
    }

    public int Id
    {
      get { return _friend.Id; }
    }

    public string FirstName
    {
      get { return _friend.FirstName; }
      set
      {
        _friend.FirstName = value;
        OnPropertyChanged();
      }
    }

    public string LastName
    {
      get { return _friend.LastName; }
      set
      {
        _friend.LastName = value;
        OnPropertyChanged();
      }
    }

    public DateTime? Birthday
    {
      get { return _friend.Birthday; }
      set
      {
        _friend.Birthday = value;
        OnPropertyChanged();
      }
    }

    public bool IsDeveloper
    {
      get { return _friend.IsDeveloper; }
      set
      {
        _friend.IsDeveloper = value;
        OnPropertyChanged();
      }
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      base.OnPropertyChanged(propertyName);
      if (propertyName != nameof(IsChanged))
      {
        IsChanged = true;
      }
    }
  }
}
