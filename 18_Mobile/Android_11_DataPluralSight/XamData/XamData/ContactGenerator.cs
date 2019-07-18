using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace XamData
{
    public class ContactGenerator
    {
        private static List<string> FirstNames = new List<string>
        {
            "Pszemek",
            "Alicja",
            "Miron",
            "Zytomir",
            "Dobromir"
        };
        private static List<string> LastNames = new List<string>
        {
            "Bak",
            "Zet",
            "Dziarlega",
            "Brzeczyszczykiewicz",
            "Dupa"
        };
        public static ObservableCollection<Contact> CreateContacts()
        {

            var random = new Random();
            var contacts = new ObservableCollection<Contact>();

            for (int i = 0; i < 25; i++)
            {
                string first = FirstNames[random.Next(FirstNames.Count - 1)];
                string last = LastNames[random.Next(LastNames.Count - 1)];
                first = InitCap(first);
                last = InitCap(last);
                Contact contact = new Contact();
                contact.FirstName = first;
                contact.LastName = last;
                contact.Email = first + "@gmail.com";
                contacts.Add(contact);
            }
            return contacts;
        }

        private static string InitCap(string value)
        {
            char[] array = value.ToCharArray();

            for (int i = 1; i < array.Length; i++)
            {
                array[i] = char.ToLower(array[i]);
            }

            if (array.Length >= 1)
            {
                array[0] = char.ToUpper(array[0]);
            }

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    array[i] = char.ToUpper(array[i]);
                }
            }
            return new string(array);
        }
    }
}
