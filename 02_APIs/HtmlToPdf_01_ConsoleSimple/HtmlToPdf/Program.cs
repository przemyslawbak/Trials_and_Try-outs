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
                    Orientation = Orientation.Landscape,
                    PaperSize = PaperKind.A4Plus,
                },
                Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = @"<dir style='color:orange;'>Lorem ipsum dolor sit amet, consectetur adipiscing elit. In consectetur mauris eget ultrices  iaculis. Ut odio viverra, molestie lectus nec, venenatis turpis.</div>",
                        WebSettings = { DefaultEncoding = "utf-8" },
                        HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
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
