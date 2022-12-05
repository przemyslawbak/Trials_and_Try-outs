using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Object;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IControlsService _controlsService;
        IObjectService _objectService;

        public MainWindowViewModel(IControlsService controlsService, IObjectService objectService)
        {
            _controlsService = controlsService;
            _objectService = objectService;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
            JedenCommand = new DelegateCommand(OnJedenCommnad);
            DwaCommand = new DelegateCommand(OnDwaCommnad);
            TrzyCommand = new DelegateCommand(OnTrzyCommnad);
        }

        public IObjectModel NewObject { get; set; }

        private void OnDwaCommnad(object obj)
        {
            CreateNewObject("Dwa");
        }

        private void OnTrzyCommnad(object obj)
        {
            CreateNewObject("Trzy");
        }

        private void OnJedenCommnad(object obj)
        {
            CreateNewObject("Jeden");
        }

        public void CreateNewObject(string firstName)
        {
            NewObject = _objectService.GetObject(firstName);
        }

        private void OnSwitchCommand(object obj)
        {
            if (Jeden == true)
            {
                UiModel = _controlsService.SwitchOff();
            }
            else
            {
                UiModel = _controlsService.SwitchOn();
            }
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
