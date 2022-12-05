using System;
using System.Collections.Generic;
using System.Text;
using XLabs.Forms.Mvvm;

namespace XamFormsMVVM.ViewModels
{
    public class ContactDetailsViewModel : ViewModel
    {
        private ContactViewModel _contact;
        public ContactViewModel Contact
        {
            get { return _contact; }
            set
            {
                _contact = value;
                NotifyPropertyChanged();
            }
        }
    }
}
