using FriendStorage.UI.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
    public class View2ViewModel : ViewModelBase
    {
        private ICommand _gotoView1Command;

        public ICommand GotoView1Command
        {
            get
            {
                return _gotoView1Command ?? (_gotoView1Command = new RelayCommand(
                   x =>
                   {
                       Mediator.NotifyColleagues("ChangeView", true);
                   }));
            }
        }
    }
}
