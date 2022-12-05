using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Main;
using Wpf_Services.Singleton;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IMainService _main;
        ISingleton _singleton = Singleton.Instance;

        public MainWindowViewModel(IMainService main)
        {
            _main = main;

            //_main.Initialize();

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
            JedenCommand = new DelegateCommand(OnJedenCommnad);
            DwaCommand = new DelegateCommand(OnDwaCommnad);
            TrzyCommand = new DelegateCommand(OnTrzyCommnad);
        }

        private void OnTrzyCommnad(object obj)
        {
            _singleton.ValueOne = 3;

            var dupa = _singleton;
        }

        private void OnDwaCommnad(object obj)
        {
            _singleton.ValueOne = 2;

            var dupa = _singleton;
        }

        private void OnJedenCommnad(object obj)
        {
            _singleton.ValueOne = 1;

            var dupa = _singleton;
        }

        private void OnSwitchCommand(object obj)
        {
            if (Jeden == true)
            {
                UiModel = _main.SwitchOff();
            }
            else
            {
                UiModel = _main.SwitchOn();
            }

            var dupa = _singleton;
        }

        public ICommand SwitchCommnad { get; private set; }
        public ICommand JedenCommand { get; private set; }
        public ICommand DwaCommand { get; private set; }
        public ICommand TrzyCommand { get; private set; }

        private UiControlsModel _uiModel;
        public UiControlsModel UiModel
        {
            get => _uiModel;
            set
            {
                _uiModel = value;
                Jeden = _uiModel.Jeden;
                Dwa = _uiModel.Dwa;
                Trzy = _uiModel.Trzy;
            }
        }

        private bool _jeden;
        public bool Jeden
        {
            get => _jeden;
            set
            {
                _jeden = value;
                OnPropertyChanged();
            }
        }

        private bool _dwa;
        public bool Dwa
        {
            get => _dwa;
            set
            {
                _dwa = value;
                OnPropertyChanged();
            }
        }

        private bool _trzy;
        public bool Trzy
        {
            get => _trzy;
            set
            {
                _trzy = value;
                OnPropertyChanged();
            }
        }
    }
}
