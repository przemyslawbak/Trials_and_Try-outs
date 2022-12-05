using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCommandDemoApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged //Inotify
    {
        public event PropertyChangedEventHandler PropertyChanged; //implementacja interfejsu INotifyPropertyChanged,

        public void OnPropertyChanged(string propertyName) //aktualizowanie kontrolek powiązanych 
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
