using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamData.ViewModel;

namespace XamData
{
    public partial class MainPage : ContentPage
    {
        MainPageViewModel vm;

        public MainPage()
        {
            vm = new MainPageViewModel();
            BindingContext = vm;
            InitializeComponent();

        }

        public void OnItemTapped(object o, ItemTappedEventArgs e)
        {
            var contact = e.Item as Contact;
            DisplayAlert("You selected {0} {1}", contact.FirstName, contact.LastName);
        }
    }
}
