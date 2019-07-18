using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testy_07_property_call_mvvm.Models
{
    public enum SendingStatus : byte { Sending, Waiting, Stopped }
    public class MsgModel : INotifyPropertyChanged
    {
        private string subjectMsg;
        public string TematWiadomosc
        {
            get
            {
                return subjectMsg;
            }
            set
            {
                subjectMsg = value;
                OnPropertyChanged("TematWiadomosc");
            }
        }

        private SendingStatus status;
        public SendingStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] propertyNames) //metoda pomocnicza
                                                                       //aktualizowanie kontrolek powiązanych 
                                                                       //z własnościami tej klasy.
                                                                       //string[] nazwyWłasności-tablica nazw własności
                                                                       //dzięki "params" nie tworzymy tablicy
        {
            if (PropertyChanged != null) //jeśli zmiana własności...
            {
                foreach (string propertyName in propertyNames) //dla każdej nazwy własności z nazwyWłasności
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
