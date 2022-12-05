using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Building;
using Wpf_Services.Controls;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IControlsService _controlsService;
        IBuilderService _normalCar;
        IBuilderService _luxuryCar;
        IBuilderService _mediumCar;

        public MainWindowViewModel(IControlsService controlsService,
            ILuxaryCarBuilder luxaryCarBuilder,
            IMediumCarBuilder mediumCarBuilder,
            INormalCarBuilder normalCarBuilder)
        {
            _controlsService = controlsService;
            _luxuryCar = luxaryCarBuilder;
            _mediumCar = mediumCarBuilder;
            _normalCar = normalCarBuilder;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
            Builder1Command = new DelegateCommand(OnBuilderLuxuryCommand);
            Builder2Command = new DelegateCommand(OnBuilderMediumCommand);
            Builder3Command = new DelegateCommand(OnBuilderNormalCommand);
        }

        public void Construct(IBuilderService car)
        {
            car.CarType();
            car.ProvideACType();
            car.ProvideColorType();
            car.ProvideUpholsteryType();
            car.ProvideWheelType();
        }

        private void OnBuilderNormalCommand(object obj)
        {
            Construct(_normalCar);

            CarProductModel normal = _normalCar.GetCar();

            normal.Display();
        }

        private void OnBuilderMediumCommand(object obj)
        {
            Construct(_mediumCar);

            CarProductModel medium = _mediumCar.GetCar();

            medium.Display();
        }

        private void OnBuilderLuxuryCommand(object obj)
        {
            Construct(_luxuryCar);

            CarProductModel luxury = _luxuryCar.GetCar();

            luxury.Display();
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
        public ICommand Builder1Command { get; private set; }
        public ICommand Builder2Command { get; private set; }
        public ICommand Builder3Command { get; private set; }

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
