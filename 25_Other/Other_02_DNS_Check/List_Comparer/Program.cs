using DnsClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace List_Comparer
{
    class Program
    {
        private static LookupClient client;
        private static int _degreeOfParallelism = 40;
        List<string> lista = new List<string>(File.ReadAllLines("_1.txt"));
        List<string> output = new List<string>();
        List<Task> tasks = new List<Task>();
        SemaphoreSlim throttler = new SemaphoreSlim(_degreeOfParallelism);

        static void Main(string[] args)
        {
            SetupClient();
            Program prog = new Program();
            prog.ExecuteAsync().Wait();
            Console.ReadLine();

        }

        private static void SetupClient()
        {
            client = new LookupClient
            {
                UseCache = true
            };
        }

        private async Task ExecuteAsync()
        {
            for (int i = 0; i < lista.Count; i++)
            {
                string status = "";
                int iteration = i;

                tasks.Add(Task.Run(async () =>
                {
                    string www = string.Empty;

                    if (IsValidEmail(lista[iteration]))
                    {
                        MailAddress address = new MailAddress(lista[iteration]);
                        www = "http://" + address.Host;
                    }
                    await throttler.WaitAsync();
                    try
                    {
                        status = await CheckAddressAsync(www);

                        if (status == "True")
                        {
                            output.Add(lista[iteration]);
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        throttler.Release();
                        Console.WriteLine("Przetworzono... " + iteration + " z " + lista.Count);
                    }
                }));
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                File.WriteAllLines("_output.txt", output);
                Console.WriteLine("saved");
            }

            Console.WriteLine("FINITO");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private async Task<string> CheckAddressAsync(string url)
        {
            string status = string.Empty;
            try
            {
                var uri = new Uri(url);
                var host = uri.Host;
                IDnsQueryResponse result = await client.QueryAsync(host, QueryType.ANY).ConfigureAwait(true);
                HttpClient clientt = new HttpClient();
                var checkingResponse = await clientt.GetAsync(url);
                if (!checkingResponse.IsSuccessStatusCode)
                {
                    status = "False";
                }
                else if (result.HasError)
                {
                    status = "False";
                }
                else
                {
                    status = "True";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                status = "False";
            }


            return status;
        }
    }
}
