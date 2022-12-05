using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Metadata;

namespace Pattern
{
    //Zasada Segregacji Interfejsów - można wybrać sobie interfejsy


    class Program
    {

    }

    public interface IPrinter
    {
        void Print(Document d);
    }

    public interface IScanner
    {
        void Scan(Document d);
    }
    public class Printer : IPrinter
    {
        public void Print(Document d)
        {
            //tutaj implementacja
        }
    }
    public class Photocopier : IPrinter, IScanner
    {
        public void Print(Document d) {}
        public void Scan(Document d) { }
    }
    public interface IMultiFunctionDevice : IPrinter, IScanner // IFax etc.
    {
        // Tutaj nie ma niczego
    }
    public class MultiFunctionMachine : IMultiFunctionDevice
    {
        //skomponuj to z kilku modułów
        private IPrinter printer;
        private IScanner scanner;
        public MultiFunctionMachine(IPrinter printer, IScanner scanner)
        {
            this.printer = printer;
            this.scanner = scanner;
        }
        public void Print(Document d)
        {
            printer.Print(d);
        }
        public void Scan(Document d)
        {
            scanner.Scan(d);
        }
    }
}
