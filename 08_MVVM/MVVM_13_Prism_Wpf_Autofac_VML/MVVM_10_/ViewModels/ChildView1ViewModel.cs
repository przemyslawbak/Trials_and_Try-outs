using MVVM_10_.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_10_.ViewModels
{
    public class ChildView1ViewModel : ViewModelBase
    {
        IEventAggregator _eventAggregator;
        public ChildView1ViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<UpdateNameEvent>().Subscribe(UpdateName);
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private void UpdateName(string name)
        {
            this.Name = name;
        }
    }
}
