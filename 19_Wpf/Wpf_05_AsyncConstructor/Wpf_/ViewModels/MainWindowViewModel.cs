using Params_Logger;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Controls;

namespace Wpf_.ViewModels
{
    //Asynchronous Initialization Pattern:
    //https://blog.stephencleary.com/2013/01/async-oop-2-constructors.html

    /// <summary>
    /// Marks a type as requiring asynchronous initialization and provides the result of that initialization.
    /// </summary>
    public interface IAsyncInitialization
    {
        /// <summary>
        /// The result of the asynchronous initialization of this instance.
        /// </summary>
        Task Initialization { get; }
    }

    public class MainWindowViewModel : ViewModelBase, IAsyncInitialization
    {
        IControlsService _controlsService;
        IParamsLogger _log;

        public MainWindowViewModel(IControlsService controlsService, IParamsLogger log)
        {
            _controlsService = controlsService;
            _log = log;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);

            Initialization = InitializeAsync();
        }

        public Task Initialization { get; private set; } //for Asynchronous Initialization Pattern

        private async Task InitializeAsync()
        {
            await Task.Delay(100000);
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
                _log.Prop(_jeden);
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
                _log.Prop(_dwa);
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
                _log.Prop(_trzy);
            }
        }
    }
}
