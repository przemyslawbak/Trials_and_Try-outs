using System;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-38. Child Task
    class Program
    {
        static void Main(string[] args)
        {
            Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Parent.");
                Task child = Task.Factory.StartNew(() => Console.WriteLine("Nested.."),
                TaskCreationOptions.AttachedToParent); //Bez AttachedToParent rodzic nie będzie czekał na 'child'. Poza tym "Any code acting on the result of the parent will see
                                                       //all the child - task exceptions as part of the aggregate exception."
                                                       //Task.Run "creates the task using the TaskCreationOptions of DenyChildAttach."
            }).Wait();
        }
    }
}
