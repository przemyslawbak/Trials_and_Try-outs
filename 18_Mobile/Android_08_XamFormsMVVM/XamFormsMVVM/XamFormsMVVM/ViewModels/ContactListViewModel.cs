using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamFormsMVVM.Data;

namespace XamFormsMVVM.ViewModels
{
    public class ContactListViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        private IList<ContactViewModel> _contacts;
        public IList<ContactViewModel> Contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                _contacts = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand ItemSelectedCommand { get; set; }
        private void OnItemSelected(ContactViewModel contactVM)
        {
            Navigation.PushAsync<ContactDetailsViewModel>((viewmodel, page) => {
                viewmodel.Contact = contactVM;
            });
        }
        public ContactListViewModel()
        {
            ItemSelectedCommand = new Command<ContactViewModel>(OnItemSelected);
            Contacts = new ObservableCollection<ContactViewModel>(
            DataService.GetAll().Select(p => new ContactViewModel(p)));
        }
    }
}
