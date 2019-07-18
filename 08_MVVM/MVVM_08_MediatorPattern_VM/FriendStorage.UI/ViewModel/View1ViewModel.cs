using FriendStorage.UI.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
    public class View1ViewModel : ViewModelBase
    {
        private ICommand _gotoView2Command;

        public ICommand GotoView2Command
        {
            get
            {
                return _gotoView2Command ?? (_gotoView2Command = new RelayCommand(
                   x =>
                   {
                       Mediator.NotifyColleagues("ChangeView", false);
                   }));
            }
        }
    }
}
