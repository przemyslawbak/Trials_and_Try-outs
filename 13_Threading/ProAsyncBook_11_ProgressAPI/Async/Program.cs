using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Async
{
    public class DataImport : IImport
    {
        public Task ImportXmlFilesAsync(string dataDirectory)
        {
            return ImportXmlFilesAsync(dataDirectory, CancellationToken.None);
        }

        public Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct, IProgress<ImportProgress> progressObserver)
        {
            return Task.Run(() =>
            {
                FileInfo[] files = new DirectoryInfo(dataDirectory).GetFiles("*.xml");
                int nFileProcessed = 0;
                foreach (FileInfo file in files)
                {
                    XElement doc = null;

                    Mutex fileMutex = new Mutex(false, file.Name);
                    bool cancelled = (WaitHandle.WaitAny(new WaitHandle[] { fileMutex, ct.WaitHandle }) == 1);


                    try
                    {
                        ct.ThrowIfCancellationRequested();
                        doc = XElement.Load(file.FullName);
                    }
                    finally
                    {
                        fileMutex.ReleaseMutex();
                    }

                    double progress = (double)nFileProcessed / (double)files.Length * 100.0;

                    progressObserver.Report(new ImportProgress((int)progress, file.Name));
                    InternalProcessXml(doc);
                    nFileProcessed++;

                }
            }, ct);
        }

        private void InternalProcessXml(XElement doc)
        {
            Thread.Sleep(1000);
        }

        public Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
    //Listing 3-31/32. Progress
    class Program
    {
        static void Main(string[] args)
        {
            IImport dataImporter = new DataImport();

            DataImport(dataImporter);
        }

        public static void DataImport(IImport import)
        {
            var tcs = new CancellationTokenSource();
            CancellationToken ct = tcs.Token;

            Task importTask = import.ImportXmlFilesAsync(@"C:\data", ct, new Progress<ImportProgress>(DisplayProgress));

            while (!importTask.IsCompleted)
            {
                Console.Write(".");
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    tcs.Cancel();
                }
                Thread.Sleep(250);
            }
        }

        private static void DisplayProgress(ImportProgress progress)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write("Processing {0} {1}% Done", progress.CurrentFile, progress.OverallProgress);
        }

        public Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct, IProgress<ImportProgress> progressObserver)
        {
            return Task.Factory.StartNew(() =>
            {
                FileInfo[] files = new DirectoryInfo(dataDirectory).GetFiles("*.xml");
                int nFileProcessed = 0;
                foreach (FileInfo file in files)
                {
                    XElement doc = XElement.Load(file.FullName);
                    double progress = (double)nFileProcessed / (double)files.Length * 100.0;
                    progressObserver.Report(new ImportProgress((int)progress, file.FullName));
                    InternalProcessXml(doc);
                    nFileProcessed++;
                    ct.ThrowIfCancellationRequested();
                }
            }, ct);
        }

        private void InternalProcessXml(XElement doc)
        {
            throw new NotImplementedException();
        }
    }

    public interface IProgress<in T>
    {
        void Report(T value);
    }

    public interface IImport
    {
        Task ImportXmlFilesAsync(string dataDirectory);
        Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct);
        Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct, IProgress<ImportProgress> progress);
    }

    public class ImportProgress
    {
        public int OverallProgress { get; private set; }
        public string CurrentFile { get; private set; }
        public ImportProgress(int overallProgress, string currentFile)
        {
            OverallProgress = overallProgress;
            CurrentFile = currentFile;
        }
    }
}
