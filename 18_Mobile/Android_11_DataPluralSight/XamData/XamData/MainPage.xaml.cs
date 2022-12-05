using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamData
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Grouping<string, Contact>> ContactGroup;
        public Contact TheContact { get; set; }
        public MainPage()
        {
            Init();
            BindingContext = ContactGroup;
            InitializeComponent();
        }
        private void Init()
        {
            var listOfContacts = ContactGenerator.CreateContacts();


            var sorted =
                from c in listOfContacts
                orderby c.FirstName
                group c by c.FirstName[0].ToString()
                into theGroup
                select new Grouping<string, Contact>(theGroup.Key, theGroup);

            ContactGroup = new ObservableCollection<Grouping<string, Contact>>(sorted);

        }
        public void OnItemTapped(object o, ItemTappedEventArgs e)
        {
            var contact = e.Item as Contact;
            DisplayAlert("You selected {0} {1}", contact.FirstName, contact.LastName);
        }
    }
}
