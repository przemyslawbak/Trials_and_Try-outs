using System;
using System.Windows;
using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Controls;

namespace Wpf_.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        IControlsService _controlsService;

        public MainWindowViewModel(IControlsService controlsService)
        {
            _controlsService = controlsService;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
            CrashMeCommand = new DelegateCommand(OnCrashMeCommand);

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            MessageBox.Show(string.Format("Runtime terminating: {0}", args.IsTerminating));
        }

        private void OnCrashMeCommand(object obj)
        {
            string a = "a";
            int i = int.Parse(a);
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
        public ICommand CrashMeCommand { get; private set; }

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
