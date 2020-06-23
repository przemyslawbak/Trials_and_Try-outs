using DinkToPdf;
using System;
using System.IO;

namespace HtmlToPdf
{
    //https://github.com/rdvojmoc/DinkToPdf
    class Program
    {
        static void Main(string[] args)
        {
            GeneratePdf();
        }

        private static void GeneratePdf()
        {
            var converter = new BasicConverter(new PdfTools());
            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    UseCompression = true //defult = true
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = @"<html style='background-color:gray;'><dir style='color:orange;'>Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Ut odio viverra, molestie lectus nec, venenatis turpis.</div></html>",
                        WebSettings = { DefaultEncoding = "utf-8" },
                        FooterSettings = { FontSize = 9, Right = "Przemyslaw Bak - Page [page] of [toPage]", Line = true}
                    }
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
