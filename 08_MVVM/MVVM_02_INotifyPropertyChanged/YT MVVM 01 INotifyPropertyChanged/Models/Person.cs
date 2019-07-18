using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT_MVVM_01_INotifyPropertyChanged.Models
{
    class Person : INotifyPropertyChanged
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value;
                OnPropertyChange("Name");
                OnPropertyChange("FullName");
            }
        }
        private string lastname;
        public string LastName
        {
            get { return lastname; }
            set { lastname = value;
                OnPropertyChange("LastName");
                OnPropertyChange("FullName");
            }
        }
        private string fullname;
        public string FullName
        {
            get { return name + " " + lastname; }
            set { fullname = value;
                OnPropertyChange("FullName");
            }
        }
        public Person()
        {

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
