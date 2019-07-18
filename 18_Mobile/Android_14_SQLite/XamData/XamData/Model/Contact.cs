using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamData
{
    public class Contact
    {

        [PrimaryKeyAttribute, AutoIncrement]
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool Favorite { get; set; }
    }
}
