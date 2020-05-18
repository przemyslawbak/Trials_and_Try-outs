using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Main;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IMainService _main;

        public MainWindowViewModel(IMainService main)
        {
            _main = main;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
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
        }

        public ICommand SwitchCommnad { get; private set; }

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
