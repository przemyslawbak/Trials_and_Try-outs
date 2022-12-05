using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows.Input;

namespace _01_DI_Example_HelloDI
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = LoadConnectionString(); //loads config values

            CurrencyParser parser = CreateCurrencyParser(connectionString); //builds object graph

            ICommand command = parser.Parse(args);
            command.Execute();
        }

        private static CurrencyParser CreateCurrencyParser(string connectionString)
        {
            return new CurrencyParser(); //composes object graph
        }

        private static string LoadConnectionString()
        {
            return "some string"; //tutaj powinn być odwołanie do ConfigurationBuilder
        }
    }

    internal class CurrencyParser
    {
        public CurrencyParser(IExchangeProvider exchangeProvider)
        {

        }
        internal ICommand Parse(string[] args)
        {
            throw new NotImplementedException();
        }
    }

    public interface IExchangeProvider
    {
    }
}
