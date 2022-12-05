#define TESTMODE // dyrektywy #define muszą znajdować się na początku pliku
// zgodnie z konwencją nazwy symboli są zapisywane dużymi literami

using System;

namespace _33_Diagnostyka
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Xml.Linq;

    class Program
    {
        //analiza wątków w procesie
        public void EnumerateThreads(Process p)
        {
            foreach (ProcessThread pt in p.Threads)
            {
                Console.WriteLine(pt.Id);
                Console.WriteLine(" State: " + pt.ThreadState);
                Console.WriteLine(" Priority: " + pt.PriorityLevel);
                Console.WriteLine(" Started: " + pt.StartTime);
                Console.WriteLine(" CPU time: " + pt.TotalProcessorTime);
            }
        }
        static void Main()
        {
#if TESTMODE && !PLAYMODE // jeżeli zdefiniowano TESTMODE i nie zdefiniowano PLAYMODE
            Console.WriteLine("W trybie testowym!"); // dane wyjściowe "W trybie testowym!"
#endif
            //debug
            Debug.Write("Dane");
            Debug.WriteLine(23 * 34);
            int x = 5, y = 3;
            Debug.WriteIf(x > y, "x jest większe niż y");

            //process
            foreach (Process p in Process.GetProcesses())
                using (p)
                {
                    Console.WriteLine(p.ProcessName);
                    Console.WriteLine(" PID: " + p.Id);
                    Console.WriteLine(" Pamięć: " + p.WorkingSet64);
                    Console.WriteLine(" Wątki: " + p.Threads.Count);
                }

            //dziennik zdarzeń
            //Microsoft.Extensions.Logging.EventLog
            EventLog log = new EventLog("Aplikacja");
            Console.WriteLine("Całkowita liczba wpisów: " + log.Entries.Count);
            EventLogEntry last = log.Entries[log.Entries.Count - 1];
            Console.WriteLine("Indeks: " + last.Index);
            Console.WriteLine("Źródło: " + last.Source);
            Console.WriteLine("Typ: " + last.EntryType);
            Console.WriteLine("Godzina: " + last.TimeWritten);
            Console.WriteLine("Komunikat: " + last.Message);

            //liczniki wydajności
            //Microsoft.Windows.Compatibility

            //Poniższy fragment kodu wyświetla wszystkie dostępne w komputerze liczniki wydajności.
            PerformanceCounterCategory[] cats = PerformanceCounterCategory.GetCategories();
            foreach (PerformanceCounterCategory cat in cats)
            {
                Console.WriteLine("Kategoria: " + cat.CategoryName);
                string[] instances = cat.GetInstanceNames();
                if (instances.Length == 0)
                {
                    foreach (PerformanceCounter ctr in cat.GetCounters())
                        Console.WriteLine(" Licznik: " + ctr.CounterName);
                }
                else // liczniki wraz z egzemplarzami
                {
                    foreach (string instance in instances)
                    {
                        Console.WriteLine(" Egzemplarz: " + instance);
                        if (cat.InstanceExists(instance))
                            foreach (PerformanceCounter ctr in cat.GetCounters(instance))
                                Console.WriteLine(" Licznik: " + ctr.CounterName);
                    }
                }
            }

            //W poniższym fragmencie kodu widać użycie omówionej metody do jednoczesnego monitorowania aktywności procesora i dysku twardego:
            EventWaitHandle stopper = new ManualResetEvent(false);
            /*new Thread(() => Monitor("Processor", "% Processor Time", "_Total", stopper)
            ).Start();
            new Thread(() => Monitor("LogicalDisk", "% Idle Time", "C:", stopper)
            ).Start();
            Console.WriteLine("Monitorowanie - naciśnij dowolny klawisz, aby zakończyć...");
            Console.ReadKey();
            stopper.Set();
            Console.ReadKey();*/
        }
    }
}
