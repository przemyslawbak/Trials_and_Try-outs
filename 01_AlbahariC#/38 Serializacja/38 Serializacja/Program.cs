using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;

namespace _38_Serializacja
{
    [Serializable] //Współpraca z atrybutem [Serializable]
                   //Atrybut [Serializable] nie jest dziedziczony, więc podklasy nie będą automatycznie
                   //umożliwiały serializacji, jeżeli nie zostaną oznaczone za pomocą omawianego atrybutu.
        [DataContract(Name = "Candidate", Namespace = "http://oreilly.com/nutshell")] //można zmienić nazwy elementów składowych danych
    public class Person
    {
        [OnDeserializing]//dla [NonSerialized] public bool Valid = true, wprowadza wartość
        void OnDeserializing(StreamingContext context)
        {
            Valid = true;
        }
        [DataMember(Name = "FirstName", Order = 1)] public string Name; //Order=1 - zamiana kolejności
        [NonSerialized] [DataMember(Name = "ClaimedAge", Order = 0)] public int Age; //atrybut 'nie serializowac'
        [NonSerialized] public bool Valid = true;
        //Istnieje możliwość zakazania serializatorowi emisji elementów składowych danych zawierających
        [DataMember(EmitDefaultValue = false)] public Address HomeAddress; //odwołanie do obiektu
    }
    //Atrybuty [OnSerializing] i [OnSerialized] - formater SOAP odmawia serializacji typów generycznych!
    [Serializable]
    public sealed class Team
    {
        public string Name;
        Person[] _playersToSerialize;
        [NonSerialized] public List<Person> Players = new List<Person>();
        [OnSerializing]
        void OnSerializing(StreamingContext context)
        {
            _playersToSerialize = Players.ToArray();
        }
        [OnSerialized]
        void OnSerialized(StreamingContext context)
        {
            _playersToSerialize = null; // pozwalamy na zwolnienie pamięci
        }
        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            Players = new List<Person>(_playersToSerialize);
        }
    }
    //Serializacja binarna za pomocą ISerializable
    [Serializable]
    public class Team2 : ISerializable
    {
        public string Name;
        public List<Person> Players;
        public virtual void GetObjectData(SerializationInfo si,
        StreamingContext sc)
        {
            si.AddValue("Name", Name);
            si.AddValue("PlayerData", Players.ToArray());
        }
        public Team2() { }
        protected Team2(SerializationInfo si, StreamingContext sc)
        {
            Name = si.GetString("Name");
            // deserializacja Players na postać tablicy, aby odpowiadała serializacji
            Person[] a = (Person[])si.GetValue("PlayerData", typeof(Person[]));
            // konstrukcja nowego egzemplarza List na podstawie tablicy
            Players = new List<Person>(a);
        }

        //i na koniec XmlSerializer, ale go już przerabialiśmy
        //jest najbardziej elastyczny z trzech wymienionych w zakresie dowolnej struktury XML
    }
    [Serializable]
    //odwołanie do obiektu
    [DataContract, KnownType(typeof(USAddress))] //atrybut KnownType
    public class Address
    {
        [DataMember] public string Street, Postcode;
    }
    [Serializable]
    [DataContract]
    public class USAddress : Address { }
    [DataContract] public class Student : Person { }
    [DataContract] public class Teacher : Person { }
    class Program
    {
        static void Main(string[] args)
        {
            //Istnieje możliwość przeprowadzenia jawnej serializacji lub deserializacji egzemplarza
            Person p = new Person { Name = "Staszek", Age = 30 };
            p.HomeAddress = new USAddress { Street = "Fawcett St", Postcode = "02138" };
            var ds = new DataContractSerializer(typeof(Person));
            using (Stream s = File.Create("person.xml"))
                ds.WriteObject(s, p); // serializacja
            Person p2;
            using (Stream s = File.OpenRead("person.xml"))
                p2 = (Person)ds.ReadObject(s); // deserializacja
            Console.WriteLine(p2.Name + " " + p2.Age); // Staszek 30

            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true }; //W przypadku XmlWriter można żądać
                                                                                    //wcięcia danych wyjściowych, co poprawia ich czytelność
            using (XmlWriter w = XmlWriter.Create("persona.xml", settings))
                ds.WriteObject(w, p);

            //serializacja podklas
            Person person = new Person { Name = "Staszek", Age = 30 };
            Student student = new Student { Name = "Staszek", Age = 30 };
            Teacher teacher = new Teacher { Name = "Staszek", Age = 30 };
            Person p3 = DeepClone(person); // OK
            //Student s2 = (Student)DeepClone(student); // zgłoszenie wyjątku SerializationException
            //Teacher t2 = (Teacher)DeepClone(teacher); // zgłoszenie wyjątku SerializationException
            var dss = new DataContractSerializer(typeof(Person), new Type[] { typeof(Student), typeof(Teacher) }); //właściwie

            //serializacja binarna
            Console.WriteLine("binarna:");
            IFormatter formatter = new BinaryFormatter();
            using (FileStream s = File.Create("serialized.bin"))
                formatter.Serialize(s, p);
            using (FileStream s = File.OpenRead("serialized.bin"))
            {
                Person p4 = (Person)formatter.Deserialize(s);
                Console.WriteLine(p4.Name + " " + p.Age); // Grzegorz 25
            }

            Console.ReadKey();
        }
        //głębokie kopiowanie
        static Person DeepClone(Person p)
        {
            var ds = new DataContractSerializer(typeof(Person));
            MemoryStream stream = new MemoryStream();
            ds.WriteObject(stream, p);
            stream.Position = 0;
            return (Person)ds.ReadObject(stream);
        }
    }
}
