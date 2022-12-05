using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Async
{
    //Listing 3-38. Child Task
    class Program
    {
        static void Main(string[] args)
        {
            //
        }

        public Task ImportXmlFilesAsync(string dataDirectory, CancellationToken ct)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (FileInfo file in new DirectoryInfo(dataDirectory).GetFiles("*.xml"))
                {
                    string fileToProcess = file.FullName;
                    Task.Factory.StartNew(_ =>
                    {
                        // convenient point to check for cancellation
                        XElement doc = XElement.Load(fileToProcess);
                        InternalProcessXml(doc, ct);
                    }, ct, TaskCreationOptions.AttachedToParent); //tworzenie 'child' zadania dla każdego pliku
                }
            }, ct);
        }

        private void InternalProcessXml(XElement doc, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
