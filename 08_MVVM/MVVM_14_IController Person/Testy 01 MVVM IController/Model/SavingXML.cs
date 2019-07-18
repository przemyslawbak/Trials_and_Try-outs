using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Testy_01_MVVM_IController.Model
{
    public static class SavingXML
    {
        //zapis do pliku
        public static void Save(string filePath, Persons persons) //zapis do pliku, wchodzi się ścieżką i instancją modelu z VM
        {
            try
            {
                XDocument xml =
                    new XDocument(new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Data zapisania: " + DateTime.Now.ToString()),
                    new XElement("Persons", from Person person in persons
                                            select new XElement("Person",
                    new XElement("ID", person.ID),
                    new XElement("FirstName", person.FirstName),
                    new XElement("SecondName", person.SecondName),
                    new XElement("Height", (byte)person.Height))));
                xml.Save(filePath);
            }
            catch (Exception exc)
            {
                throw new Exception("Error when saving XML file", exc);
            }
        }
        public static Persons Load(string filePath)
        {
            try
            {
                XDocument xml = XDocument.Load(filePath);
                IEnumerable<Person> dane =
                from zadanie in xml.Root.Descendants("Person")
                select new Person
                (
                zadanie.Element("ID").Value,
                zadanie.Element("FirstName").Value,
                zadanie.Element("SecondName").Value,
                (HeightList)byte.Parse(zadanie.Element("Height").Value));
                Persons persons = new Persons();
                foreach (Person person in dane) persons.NewPerson(person);
                return persons;
            }
            catch (Exception exc)
            {
                throw new Exception("Error when reading XML file", exc);
            }
        }
    }
}
