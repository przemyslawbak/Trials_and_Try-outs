using FriendStorage.UI.Model;
using FriendStorage.UI.Services;
using FriendStorage.UI.View;
using System;

namespace FriendStorage.UI.ViewModel
{
    //https://www.technical-recipes.com/2016/using-the-mediator-pattern-in-mvvm-wpf/
    public class MainViewModel : ViewModelBase
    {
        private object _view1 = new View1();
        private object _view2 = new View2();
        private object _currentView;
        public MainViewModel()
        {
            _currentView = _view1;
            Mediator.Register("ChangeView", OnChangeView);
            Execute();
        }

        public object CurrentView
        {
            get
            {
                return _currentView;
            }
            set
            {
                _currentView = value;
                OnPropertyChanged("CurrentView");
            }
        }

        public void OnChangeView(object show)
        {
            bool showView1 = (bool)show;
            CurrentView = showView1 ? _view1 : _view2;
        }

        private void Execute()
        {
            //string dupa = _someService.GetTheName(_personService);
            //System.Windows.MessageBox.Show(Name);
        }
    }
}
