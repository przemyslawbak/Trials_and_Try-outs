using System;
using System.Collections.Generic;
using System.Text;
using XamFormsMVVM.Models;

namespace XamFormsMVVM.Data
{
    public static class DataService
    {
        public static IEnumerable<Contact> GetAll()
        {
            return new List<Contact> {
                new Contact() {
                FirstName = "Jan",
                LastName = "Nowak",
                Profession = "Informatyk"
                },
                new Contact() {
                FirstName = "Zbigniew",
                LastName = "Kowalski",
                Profession = "Programista"
                }
            };
        }
    }
}
