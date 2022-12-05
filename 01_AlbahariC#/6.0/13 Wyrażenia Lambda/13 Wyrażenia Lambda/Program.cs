using System;

namespace _13_Wyrażenia_Lambda
{
    delegate int Transformer(int i); //typ delegacyjny
    class Program
    {
        //jawne określenie typów
        static void Foo<T>(T x) { }
        static void Bar<T>(Action<T> a) { }
        static void Main(string[] args)
        {
            Transformer sqr = x => x * x;
            Console.WriteLine(sqr(3)); // 9
            Func<int, int> ttt = x => x * x; //najczęściej używa się z delegatami Func i Action
            Console.WriteLine(ttt(4)); // 16
            Func<string, string, int> totalLength = (s1, s2) => s1.Length + s2.Length; //dwa parametry
            int total = totalLength("witaj", "świecie"); // zmienna total ma wartość 12
            Bar((int x) => Foo(x));
            //zmienne zewnętrzne
            int factor = 2; //przechwycona zmienna
            Func<int, int> multiplier = n => n * factor;
            factor = 10; //Wartości przechwyconych zmiennych są obliczane w chwili wywoływania delegatu
            Console.WriteLine(multiplier(3)); // 30 <-wywołanie delegatu
            //lambda moze zmieniac przechwycone zmienne
            int seed = 0; //przechwycona zmienna
            Func<int> natural = () => seed++; //lambda
            Console.WriteLine(natural()); // 0
            Console.WriteLine(natural()); // 1
            Console.WriteLine(natural()); // 2
            Console.WriteLine(natural()); // 3
            Console.WriteLine(natural()); // 4
            Console.WriteLine(seed); // 5
            //przechwycenie zmiennych iteracyjnych
            Action[] actions = new Action[3];
            int i = 0;
            actions[0] = () => Console.Write(i);
            i = 1;
            actions[1] = () => Console.Write(i);
            i = 2;
            actions[2] = () => Console.Write(i);
            i = 3;// wartość wywołania
            foreach (Action a in actions) a(); // 333
            Action[] actionss = new Action[3];
            for (int j = 0; j < 3; j++)
            {
                int loopScopedi = j; //przypisanie zmiennej iteracyjnej do zmiennej lokalnej o zakresie dostępności ograniczonym do wnętrza pętli
                actionss[j] = () => Console.Write(loopScopedi);
            }
            foreach (Action a in actions) a(); // 012
            Console.ReadKey();
        }
    }
}
