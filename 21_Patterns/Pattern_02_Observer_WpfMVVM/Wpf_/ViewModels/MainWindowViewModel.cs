using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using Wpf_.Commands;
using Wpf_Models;
using Wpf_Services.Controls;
using Wpf_Services.Observer;

namespace Wpf_.ViewModels
{
    interface ISubject
    {
        void Subscribe(IObserverService observer);
        void Unsubscribe(IObserverService observer);
        void Notify();
    }

    public class MainWindowViewModel : ViewModelBase, ISubject
    {
        private readonly IControlsService _controlsService;
        private readonly IObserverService _observerService;
        private ArrayList _observers;

        public MainWindowViewModel(IControlsService controlsService, IObserverService observerService)
        {
            _controlsService = controlsService;
            _observerService = observerService;

            SwitchCommnad = new DelegateCommand(OnSwitchCommand);
            ObserverCommand = new DelegateCommand(OnObserverCommand);

            InitProgram();
        }

        private void InitProgram()
        {
            _observers = new ArrayList();

            IObserverService observer1 = new ObserverService("Observer 1");
            IObserverService observer2 = new ObserverService("Observer 2");

            _observers.Add(observer1);
            _observers.Add(observer2);
        }

        public List<IObserverService> Observers { get; set; }

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

        private void OnObserverCommand(object obj)
        {
            SomeValue++;
        }

        public void Subscribe(IObserverService observer)
        {
            Observers.Add(observer);
        }

        public void Unsubscribe(IObserverService observer)
        {
            Observers.Remove(observer);
        }

        public void Notify()
        {
            Observers.ForEach(x => x.Update(SomeValue));
        }

        public ICommand SwitchCommnad { get; private set; }
        public ICommand ObserverCommand { get; private set; }

        private int _someValue;
        public int SomeValue
        {
            get => _someValue;
            set
            {
                _someValue = value;
                InformObservers();
            }
        }

        private void InformObservers()
        {
            foreach (IObserverService x in _observers)
            {
                x.Update(SomeValue);
            }
        }

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
