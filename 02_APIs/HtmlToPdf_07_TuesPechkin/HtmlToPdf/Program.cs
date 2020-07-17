using System;
using System.Drawing.Printing;
using System.IO;
using TuesPechkin;

namespace HtmlToPdf
{
    class Program
    {
        static void Main(string[] args)
        {
            GeneratePdf();
        }

        private static void GeneratePdf()
        {
            IConverter converter =
    new ThreadSafeConverter(
        new PdfToolset(
            new Win32EmbeddedDeployment(
                new TempFolderDeployment())));

            HtmlToPdfDocument doc = new HtmlToPdfDocument()
            {
                GlobalSettings =
                {
                    ProduceOutline = true,
                    DocumentTitle = "Pretty stuff",
                    PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
                    Margins =
                    {
                        All = 1.375,
                        Unit = Unit.Centimeters
                    }
                },
                Objects = {
                    new ObjectSettings { HtmlText = @"<html style='background-color:gray;'><dir style='color:orange;'>Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Ut odio viverra, molestie lectus nec, venenatis turpis.</div></html>" }
                }
            };

            byte[] pdf = converter.Convert(doc);

            ByteArrayToFile("dupa.pdf", pdf);
        }

        public static void ByteArrayToFile(string fileName, byte[] byteArray)
        {
            try
            {
                using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }
        }
    }
}
