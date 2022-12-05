using System;
using System.Collections;

namespace TutorialsPt_01e_System.Collections
{
    class Program //https://www.tutorialspoint.com/csharp/csharp_queue.htm
    {
        static void Main(string[] args)
        {
            Queue q = new Queue();

            q.Enqueue('A'); //dodawanie na końcu
            q.Enqueue('M');
            q.Enqueue('G');
            q.Enqueue('W');

            Console.WriteLine("Current queue: ");
            foreach (char c in q) Console.Write(c + " ");

            Console.WriteLine();
            q.Enqueue('V');
            q.Enqueue('H');
            Console.WriteLine("Current queue: ");
            foreach (char c in q) Console.Write(c + " ");

            Console.WriteLine();
            Console.WriteLine("Removing some values ");
            char ch = (char)q.Dequeue(); //usuwanie na początku
            Console.WriteLine("The removed value: {0}", ch); //zwracanie usuniętej
            ch = (char)q.Dequeue();
            Console.WriteLine("The removed value: {0}", ch);

            Console.ReadKey();
        }
    }
}
