using System;
using System.Collections.Generic;

namespace Pattern
{
    //Prototyp

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }

    interface IDeepCopyable<T>
    {
        T DeepCopy();
    }

    public class Person : IDeepCopyable<Person>
    {
        public string[] Names;
        public Address Address;
        public Person DeepCopy()
        {
            var copy = new Person();
            copy.Names = Array.Copy(Names); // string[] nie implementuje interfejsu
            IDeepCopyable copy.Address = Address.DeepCopy(); // Address implementuje interfejs
            IDeepCopyable
        return copy;
        }
        //tutaj inne składowe
    }

    public class Address
    {
        public readonly string StreetName;
        public int HouseNumber;
        public Address(string streetName, int houseNumber) { ... }
    }
}
