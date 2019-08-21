using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Dialogs;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IControlsService _controlsService;
        private readonly IDialogService _dialogService;

        public MainWindowViewModel(IControlsService controlsService, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _controlsService = controlsService;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
            OpenDialogCommand = new DelegateCommand(OnOpenDialogCommand);
        }

        private void OnOpenDialogCommand(object obj)
        {
            var viewModel = new DialogViewModel("Hello!");

            bool? result = _dialogService.ShowDialog(viewModel);

            if (result.HasValue)
            {
                if (result.Value)
                {
                    // Accepted
                }
                else
                {
                    // Cancelled
                }
            }
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
        public ICommand OpenDialogCommand { get; private set; }

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
