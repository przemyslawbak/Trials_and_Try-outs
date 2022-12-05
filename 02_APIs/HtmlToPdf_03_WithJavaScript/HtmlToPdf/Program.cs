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
            HtmlToPdfDocument doc = new HtmlToPdfDocument()
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
                        HtmlContent = GetHtml(),
                        WebSettings = { DefaultEncoding = "utf-8", EnableJavascript = true,  },
                        FooterSettings = { FontSize = 9, Right = "Przemyslaw Bak - Page [page] of [toPage]", Line = true}
                    }
                }
            };

            byte[] pdf = converter.Convert(doc);

            ByteArrayToFile("dupa.pdf", pdf);
        }

        private static string GetHtml()
        {
            return @"<html style='background-color:gray;'>
                        <div id='stepsId'>
  <ol>
    <li id='li1' style='color: red;'><b>Step 1</b></li>
      <li id='li2'><b>Step 2</b></li>
    <li id='li3'><b>Step 3</b></li>
  </ol>
</div>
</html>

<script>
var lis = document.getElementById('li1')
lis.style.color = 'pink';
</script>";
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
