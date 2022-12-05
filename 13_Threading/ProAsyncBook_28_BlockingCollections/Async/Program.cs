using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    //Listing 5-22/23. Blocking Producer/Consumer, Bounded Collection

    public class Program
    {
        static void Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://+:9000/Time/");
            listener.Start();
            var requestQueue = new BlockingCollection<HttpListenerContext>(new ConcurrentQueue<HttpListenerContext>(), 2);
            var producer = Task.Run(() => Producer(requestQueue, listener));
            Task[] consumers = new Task[4];
            for (int nConsumer = 0; nConsumer < consumers.Length; nConsumer++)
            {
                consumers[nConsumer] = Task.Run(() => Consumer(requestQueue));
            }
            Console.WriteLine("Listening..");
            producer.Wait();
            Task.WaitAll(consumers);
        }
        public static void Producer(BlockingCollection<HttpListenerContext> queue, HttpListener listener)
        {
            while (true)
            {
                HttpListenerContext ctx = listener.GetContext();
                if (ctx.Request.QueryString.AllKeys.Contains("stop")) break;

                if (!queue.TryAdd(ctx))
                {
                    ctx.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                    ctx.Response.Close();
                }
            }

            queue.CompleteAdding();
            Console.WriteLine("Producer stopped");
        }
        public static void Consumer(BlockingCollection<HttpListenerContext> queue)
        {
            try
            {
                while (true)
                {
                    HttpListenerContext ctx = queue.Take();
                    Console.WriteLine(ctx.Request.Url);
                    Thread.Sleep(5000);
                    using (var writer = new StreamWriter(ctx.Response.OutputStream))
                    {
                        writer.WriteLine(DateTime.Now);
                    }
                }
            }
            catch (InvalidOperationException error) { Console.WriteLine(error.Message); }
            Console.WriteLine("{0}:Stopped", Task.CurrentId);
        }
    }
}
