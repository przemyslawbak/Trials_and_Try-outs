using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamData.View
{
	public partial class ContactEntry : ContentPage
    {

        private int updateID = 0;

        public ContactEntry(int id)
        {
            InitializeComponent();
            var contact = App.Database.GetContect(id);
            FirstName.Text = contact.FirstName;
            LastName.Text = contact.LastName;
            Email.Text = contact.Email;
            IsFavorite.IsToggled = contact.Favorite;
            updateID = id;

        }

        public ContactEntry()
        {
            InitializeComponent();
        }

        public void OnSave(object o, EventArgs e)
        {
            Contact contact = new Contact();
            contact.FirstName = FirstName.Text;
            contact.LastName = LastName.Text;
            contact.Email = Email.Text;
            contact.Favorite = IsFavorite.IsToggled;
            contact.ID = updateID;
            Clear();
            App.Database.SaveContact(contact);
            Navigation.PushAsync(new MainPage());
        }

        private void OnCancel(object o, EventArgs e)
        {
            Clear();
        }

        private void OnReview(object o, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        private void Clear()
        {
            FirstName.Text = LastName.Text = Email.Text = string.Empty;
            IsFavorite.IsToggled = false;
        }
    }
}