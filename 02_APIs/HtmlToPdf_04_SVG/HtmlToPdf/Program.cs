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
            return @"<html style='background-color:gray;'><div class='container'>
    <div class='row'>
        <div class='col-sm-3'>
            <p>The g and use elements</p>
            <svg width = '200' height='200'>

                <rect width = '100%' height='100%' fill='url(#pxGrid)'
                      stroke='gray' stroke-width='1' />

                <!--grupowanie-->
                <g id = 'house' stroke='black' stroke-width='2' fill='red'
                   fill-opacity='0.5'>
                    <path d = 'M50 10 90 40 H10 Z' />
                    < rect x='10' y='40' width='80' height='50' />
                    <rect x = '60' y='60' width='20' height='30' fill='none' />
                </g>

                <!--duplikacja-->
                <use xlink:href='#house' x='100' y='0' />
                <use xlink:href='#house' x='0' y='100' />
                <use xlink:href='#house' x='100' y='100' />
            </svg>
        </div>
        <div class='col-sm-3'>
            <p>Translate in x</p>
            <svg width = '200' height='200'>
                <rect width = '100%' height='100%' fill='url(#pxGrid)'
                      stroke='gray' stroke-width='1' />
                <use xlink:href='#house'>
                    <!--animacja-->
                    <animateTransform attributeName = 'transform'
                                      type='translate'
                                      from='0 0'
                                      to='100 0'
                                      begin='0s'
                                      dur='5s'
                                      repeatCount='indefinite' />
                </use>
            </svg>
        </div>
        <div class='col-sm-3'>
            <p>Scale in x,y</p>
            <svg width = '200' height='200'>
                <rect width = '100%' height='100%' fill='url(#pxGrid)'
                      stroke='gray' stroke-width='1' />
                <use xlink:href='#house'>
                    <!--animacja-->
                    <animateTransform attributeName = 'transform'
                                      type='scale'
                                      from='1,1'
                                      to='2,2'
                                      begin='0s'
                                      dur='5s'
                                      repeatCount='indefinite' />
                </use>
            </svg>
        </div>
        <div class='col-sm-3'>
            <p>Scale in x,y</p>
            <svg width = '200' height='200'>
                <rect width = '100%' height='100%' fill='url(#pxGrid)'
                      stroke='gray' stroke-width='1' />
                <use xlink:href='#house'>
                    <animateTransform attributeName = 'transform'
                                      type='rotate'
                                      from='-45'
                                      to='45'
                                      begin='0s'
                                      dur='5s'
                                      repeatCount='indefinite' />
                </use>
            </svg>
        </div>
        <div class='col-sm-3'>
            <p>skewX(45)</p>
            <svg width = '200' height='200' viewBox='0,0,200,200'>
                <rect width = '100%' height='100%' fill='url(#pxGrid)'
                      stroke='gray' stroke-width='1' />
                <use xlink:href='#house'>
                    <animateTransform attributeName = 'transform'
                                      type='skewY'
                                      from='0'
                                      to='45'
                                      begin='0s'
                                      dur='5s'
                                      repeatCount='indefinite' additive='sum' />
                    <animateTransform attributeName = 'transform'
                                      type='skewX'
                                      from='0'
                                      to='45'
                                      begin='0s'
                                      dur='5s'
                                      repeatCount='indefinite' additive='sum' />
                </use>
            </svg>
        </div>
    </div>
</div>
</html>";
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
