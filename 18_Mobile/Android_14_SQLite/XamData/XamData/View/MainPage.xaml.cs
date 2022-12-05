using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamData.View;
using XamData.ViewModel;

namespace XamData
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public void OnItemTapped(object o, ItemTappedEventArgs e)
        {
            var contact = e.Item as Contact;
            Navigation.PushAsync(new ContactEntry(contact.ID));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ContactList.ItemsSource = App.Database.GetContacts();
        }

        protected void OnNewEntry(object o, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}
