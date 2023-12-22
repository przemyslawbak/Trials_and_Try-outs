using System;
using System.Collections.Generic;

delegate T NumberChanger<T>(T n); //generic delegate

namespace TutorialsPt_02_Collections.Generic //https://www.tutorialspoint.com/csharp/csharp_generics.htm
{
    class TestDelegate
    {
        static int num = 10;

        public static int AddNum(int p)
        {
            num += p;
            return num;
        }
        public static int MultNum(int q)
        {
            num *= q;
            return num;
        }
        public static int getNum()
        {
            return num;
        }
        public class MyGenericArray<T> //generic class
        {
            private T[] array;

            public MyGenericArray(int size)
            {
                array = new T[size + 1];
            }
            public T getItem(int index)
            {
                return array[index];
            }
            public void setItem(int index, T value)
            {
                array[index] = value;
            }

        }

        class Program
        {
            static void Swap<T>(ref T lhs, ref T rhs) //generic method
            {
                T temp;
                temp = lhs;
                lhs = rhs;
                rhs = temp;
            }
            static void Main(string[] args)
            {
                MyGenericArray<int> intArray = new MyGenericArray<int>(5);

                for (int i = 0; i < 5; i++)
                {
                    intArray.setItem(i, i * 5);
                }

                //retrieving the values
                for (int i = 0; i < 5; i++)
                {
                    Console.Write(intArray.getItem(i) + " ");
                }

                Console.WriteLine();

                //declaring a character array
                MyGenericArray<char> charArray = new MyGenericArray<char>(5);

                //setting values
                for (int i = 0; i < 5; i++)
                {
                    charArray.setItem(i, (char)(i + 98));
                }

                //retrieving the values
                for (int c = 0; c < 5; c++)
                {
                    Console.Write(charArray.getItem(c) + " ");
                }
                Console.WriteLine("Następne metoda generic");
                Console.ReadKey();

                int a, b;
                char e, d;
                a = 10;
                b = 20;
                e = 'I';
                d = 'V';
                //display values before swap:
                Console.WriteLine("Int values before calling swap:");
                Console.WriteLine("a = {0}, b = {1}", a, b);
                Console.WriteLine("Char values before calling swap:");
                Console.WriteLine("e = {0}, d = {1}", e, d);

                //call swap
                Swap<int>(ref a, ref b);
                Swap<char>(ref e, ref d);

                //display values after swap:
                Console.WriteLine("Int values after calling swap:");
                Console.WriteLine("a = {0}, b = {1}", a, b);
                Console.WriteLine("Char values after calling swap:");
                Console.WriteLine("e = {0}, d = {1}", e, d);


                Console.WriteLine("Teraz delegat");
                Console.ReadKey();

                //create delegate instances
                NumberChanger<int> nc1 = new NumberChanger<int>(AddNum);
                NumberChanger<int> nc2 = new NumberChanger<int>(MultNum);

                //calling the methods using the delegate objects
                nc1(25);
                Console.WriteLine("Value of Num: {0}", getNum());

                nc2(5);
                Console.WriteLine("Value of Num: {0}", getNum());

                Console.ReadKey();
            }
        }
    }
}
