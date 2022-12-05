using System;
using System.Collections;

namespace TutorialsPt_01d_System.Collections
{
    class Program // https://www.tutorialspoint.com/csharp/csharp_stack.htm
    {
        static void Main(string[] args)
        {
            Stack st = new Stack();

            st.Push('A'); //dodawanie
            st.Push('M');
            st.Push('G');
            st.Push('W');

            Console.WriteLine("Current stack: ");
            foreach (char c in st)
            {
                Console.Write(c + " ");
            }
            Console.WriteLine();

            st.Push('V');
            st.Push('H');
            Console.WriteLine("The next poppable value in stack, after adding V, H: {0}", st.Peek()); //zwracanie elemetu z góy
            Console.WriteLine("Current stack: ");

            foreach (char c in st)
            {
                Console.Write(c + " ");
            }

            Console.WriteLine();

            Console.WriteLine("Removing values ");
            st.Pop(); //usuwanie
            st.Pop();
            st.Pop();

            Console.WriteLine("Current stack: ");
            foreach (char c in st)
            {
                Console.Write(c + " ");
            }

            Console.ReadKey();
        }
    }
}
