using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Testy_07_property_call_mvvm.ViewModels
{
    public class MsgViewModel : INotifyPropertyChanged
    {
        private Models.MsgModel message = new Models.MsgModel();

        public MsgViewModel()
        {
            Status = Models.SendingStatus.Stopped;
        }
        

        



        public string TematWiadomosc
        {
            get
            {
                return message.TematWiadomosc;
            }
            set
            {
                message.TematWiadomosc = value;
            }

        }

        public Models.SendingStatus Status
        {
            get
            {
                return message.Status;
            }
            set
            {
                message.Status = value;
            }
        }

        private ICommand okno;
        public ICommand Okno
        {
            get
            {
                if (okno == null)
                    okno = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        var openDialog = new Powiadomienie();
                        openDialog.ShowPowiadomienie(TematWiadomosc, "Powiadomienie");
                    });
                return okno;
            }
        }

        private ICommand start;


        public ICommand Start
        {
            get
            {
                if (start == null)
                    start = new RelayCommand(
                    o => //parametr "o" wyrażenia lambda
                    {
                        Egzecue();
                    });
                return start;
            }
        }

        public void Egzecue()
        {
            Status = Models.SendingStatus.Sending;
            OnPropertyChanged("Status");
            var openDialog = new Powiadomienie();
            openDialog.ShowPowiadomienie(Status.ToString(), "Powiadomienie");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(params string[] nazwyWłasności) //metoda pomocnicza
                                                                       //aktualizowanie kontrolek powiązanych 
                                                                       //z własnościami tej klasy.
                                                                       //string[] nazwyWłasności-tablica nazw własności
                                                                       //dzięki "params" nie tworzymy tablicy
        {
            if (PropertyChanged != null) //jeśli zmiana własności...
            {
                foreach (string nazwaWłasności in nazwyWłasności) //dla każdej nazwy własności z nazwyWłasności
                    PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłasności));
            }
        }
    }
}
