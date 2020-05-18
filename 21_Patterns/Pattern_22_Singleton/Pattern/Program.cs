using System;
using System.Collections.Generic;

namespace Pattern
{
    //Singleton

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }
    /// <summary>
    /// Dzięki zadeklarowaniu egzemplarza jako statycznego usunęliśmy możliwości zarządzania czasem
    /// życia bazy danych.Teraz „żyje” ona tak długo, jak program.
    /// </summary>
    public class Database
    {
        private static int instanceCount = 0;
        private Database()
        {
            if (++instanceCount > 1) //jeśli więcej niż 1
                Console.WriteLine("Nie można stworzyć >1 bazy danych!");
        }

        public static Database Instance { get; } = new Database();
    };

    //testowanie
    public class ConfigurableRecordFinder
    {
        private IDatabase database;
        public ConfigurableRecordFinder(IDatabase database)
        {
            this.database = database;
        }
        public int GetTotalPopulation(IEnumerable<string> names)
        {
            int result = 0;
            foreach (var name in names)
                result += database.GetPopulation(name);
            return result;
        }
    }

    //testowanie
    public interface IDatabase
    {
        int GetPopulation(string name);
    }

    //testowanie
    public class DummyDatabase : IDatabase
    {
        public int GetPopulation(string name)
        {
            return new Dictionary<string, int>
            {
                ["alpha"] = 1,
                ["beta"] = 2,
                ["gamma"] = 3
            }[name];
        }
    }

    //ew test jednostkowy
    [Test]
    public void DependentTotalPopulationTest()
    {
        var db = new DummyDatabase();
        var rf = new ConfigurableRecordFinder(db);
        Assert.That(
        rf.GetTotalPopulation(new[] { "alpha", "gamma" }),
        Is.EqualTo(4));
    }
}
