using System;
using System.Collections.Generic;
using System.Text;

namespace XamData.ViewModel
{
    public class MainPageViewModel
    {

        public List<Contact> Contacts { get; set; }

        public MainPageViewModel()
        {

            Contacts = new List<Contact>();
            var listOfContacts = ContactGenerator.CreateContacts();
            Contacts = listOfContacts;

        }
    }
}
