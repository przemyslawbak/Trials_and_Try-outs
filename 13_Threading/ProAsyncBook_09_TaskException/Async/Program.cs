using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Async
{
    //Listing 3-17/18/19. try-catch task exception
    class Program
    {
        static void Main(string[] args)
        {
            Task task = Task.Factory.StartNew(() => Import(@"C:\data\2.xml"));
            try
            {
                task.Wait();
            }
            catch (AggregateException errors)
            {
                foreach (Exception error in errors.InnerExceptions)
                {
                    Console.WriteLine("{0} : {1}", error.GetType().Name, error.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : {0}", e);
            }
        }

        private static void Import(string fullName)
        {
            XElement doc = XElement.Load(fullName);
            // process xml document
        }
    }
}
