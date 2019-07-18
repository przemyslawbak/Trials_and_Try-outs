using System;
using System.Diagnostics;

namespace _24_Okreslanie_kolejnosci//i klasy pomocnicze
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] colors = { "Green", "Red", "Blue" };
            Array.Sort(colors);
            foreach (string c in colors) Console.Write(c + " "); // Blue Green Red

            Console.WriteLine("Beck".CompareTo("Anne")); // 1
            Console.WriteLine("Beck".CompareTo("Beck")); // 0
            Console.WriteLine("Beck".CompareTo("Chris")); // –1

            //klasy pomocnicze

            //klasa console
            Console.WindowWidth = Console.LargestWindowWidth;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("test... 50%");
            Console.CursorLeft -= 3;
            Console.Write("90%"); // test... 90%

            // najpierw należy zapisać istniejący obiekt
            System.IO.TextWriter oldOut = Console.Out;
            // przekierowanie wyjścia konsoli do pliku
            using (System.IO.TextWriter w = System.IO.File.CreateText
            ("c:\\output.txt"))
            {
                Console.SetOut(w);
                Console.WriteLine("Witaj, świecie");
            }
            // przywrócenie standardowego wyjścia konsoli
            Console.SetOut(oldOut);

            //klasa process
            Process.Start("notepad.exe", "c:\\output.txt");

            //klasa appcontext
            AppContext.SetSwitch("MyLibrary.SomeBreakingChange", true);
            bool isDefined, switchValue;
            isDefined = AppContext.TryGetSwitch("MyLibrary.SomeBreakingChange",
            out switchValue);


            Console.ReadKey();
        }
    }
}
