using System;

namespace Unit_Test_10___DeepStub
{
    //based on https://stackoverflow.com/questions/10495480/how-write-stub-method-with-nunit-in-c-sharp
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
    public class FirstDeep
    {
        readonly ISecondDeep secondDeep; //egzemplarz interfejsu
        public FirstDeep(ISecondDeep secDeep) //injekcja w konstruktorze
        {
            secDeep = secondDeep; //przypisanie parametru
        }

        public string AddA(string str) //metoda
        {
            SecondDeep sd = new SecondDeep(); //instancja
            bool flag = sd.SomethingToDo(str); //wywołanie metody

            if (flag == true)
                str = string.Concat(str, "AAA");
            else
                str = string.Concat(str, "BBB");

            return str;
        }
    }
    public class SecondDeep : ISecondDeep //implementuje interfejs
    {
        public bool SomethingToDo(string str)
        {
            bool flag = false;
            if (str.Length < 10)
            {
                //todo something in DB, and after that flag should be TRUE
            }
            return flag;
        }
    }
    public interface ISecondDeep //zastępujemy drugą klasę interfejsem który będzie ją symulował
    {
        bool SomethingToDo(string str); //symuluje metodę z klasy
    }
}
