using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;

namespace Pattern
{
    //Zasada zależności (od absrakcji) - odwrócenia zależności


    class Program
    {

    }

    public class Person
    {
        public string Name;
        // Tutaj data urodzenia i inne przydatne właściwości
    }

    public enum Relationship
    {
        Parent,
        Child,
        Sibling
    }

    public interface IRelationshipBrowser
    {
        IEnumerable<Person> FindAllChildrenOf(string name);
    }

    public class Relationships : IRelationshipBrowser //niski poziom
    {
        //te metody nie są już publiczne!
        private List<(Person, Relationship, Person)> relations = new List<(Person, Relationship, Person)>();
        public IEnumerable<Person> FindAllChildrenOf(string name)
        {
            return relations
            .Where(x => x.Item1.Name == name
            && x.Item2 == Relationship.Parent)
            .Select(r => r.Item3);
        }
    }
}
