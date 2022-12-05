using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Async
{
    //Listing 3-36. Asynchronous Fork and Join
    class Program
    {
        static void Main(string[] args)
        {
            Task[] algorithmTasks = new Task[4];
            for (int nTask = 0; nTask < algorithmTasks.Length; nTask++)
            {
                int partToProcess = nTask;
                algorithmTasks[nTask] = Task.Factory.StartNew(() => ProcessPart(partToProcess));
            }
            Task.Factory.ContinueWhenAll(algorithmTasks, antecedentTasks => ProduceSummary());
        }

        private static void ProduceSummary()
        {
            throw new NotImplementedException();
        }

        private static void ProcessPart(int partToProcess)
        {
            throw new NotImplementedException();
        }
    }
}
