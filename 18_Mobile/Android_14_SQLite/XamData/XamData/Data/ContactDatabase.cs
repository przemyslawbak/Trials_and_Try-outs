using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace XamData.Data
{
    public class ContactDatabase
    {

        SQLiteConnection database;
        static object locker = new object();

        public ContactDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Contact>();
        }

        public int SaveContact(Contact contact)
        {
            lock (locker)
            {
                if (contact.ID != 0)
                {
                    database.Update(contact);
                    return contact.ID;
                }
                else
                {
                    return database.Insert(contact);
                }
            }
        }
        public IEnumerable<Contact> GetContacts()
        {
            lock (locker)
            {
                return (from c in database.Table<Contact>()
                        select c).ToList();
            }
        }
        public Contact GetContect(int id)
        {
            lock (locker)
            {
                return database.Table<Contact>().Where(c => c.ID == id).FirstOrDefault();
            }
        }

        public int DeleteContact(int id)
        {
            lock (locker)
            {
                return database.Delete<Contact>(id);
            }
        }
    }
}
