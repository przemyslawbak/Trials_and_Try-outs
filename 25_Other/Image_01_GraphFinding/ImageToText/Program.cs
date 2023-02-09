using AForge.Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageToText
{
    class Program
    {
        private static List<PixelData> _curvePixels = new List<PixelData>();
        private static List<PixelData> _gridPixels = new List<PixelData>();
        private static List<PixelData> _labelPixels = new List<PixelData>();
        static void Main(string[] args)
        {
            string path = Path.Combine("chart.png");
            Bitmap bmp = new Bitmap(path);

            GetPixels(bmp);

            _curvePixels = _curvePixels
                .GroupBy(x => x.X)
                .Select(g => new PixelData { Color = g.FirstOrDefault().Color, X = g.FirstOrDefault().X, Y = (int)g.Average(x => x.Y) })
                .ToList();

            var maxYgraph = _gridPixels.Where(x => x.X == 50).Select(x => x.Y).Max();
            var minYgraph = _gridPixels.Where(x => x.X == 50).Select(x => x.Y).Min();
            var maxYlabelLine = maxYgraph;
            var minYlabelLine = minYgraph;
            var maxXgraph = _gridPixels.Where(y => y.Y == maxYgraph).Select(y => y.X).Max();
            var minXgraph = _gridPixels.Where(y => y.Y == maxYgraph).Select(y => y.X).Min();
            var maxXlabelLine = _gridPixels.Where(y => y.Y == maxYgraph + 2).Select(y => y.X).Max();
            var minXlabelLine = _gridPixels.Where(y => y.Y == maxYgraph + 2).Select(y => y.X).Min();
            var oYlabelsXmin = maxXgraph + 1;
            var oYlabelsXmax = bmp.Width;
            var oXlabelsYmax = minYgraph;
            var oYlabelHeight = 24;
            var oXlabelHeight = 24;
            var oXlabelWidth = 80;

            var oYmaxRect = new RectangleF(oYlabelsXmin, maxYlabelLine + oYlabelHeight/2, oYlabelsXmax - oYlabelsXmin, oYlabelHeight);
            var oYminRect = new RectangleF(oYlabelsXmin, minYlabelLine + oYlabelHeight / 2, oYlabelsXmax - oYlabelsXmin, oYlabelHeight);
            var oXmaxRect = new RectangleF(maxXlabelLine - oXlabelWidth/2, oXlabelsYmax, oXlabelWidth, oXlabelHeight);
            var oXminRect = new RectangleF(maxXlabelLine - oXlabelWidth / 2, oXlabelsYmax, oXlabelWidth, oXlabelHeight);

            var oYmaxValueImage = CropImage(bmp, oYmaxRect);
            var oYminValueImage = CropImage(bmp, oYminRect);
            var oXmaxValueImage = CropImage(bmp, oXmaxRect);
            var oXminValueImage = CropImage(bmp, oXminRect);

            var templatesDict = new Dictionary<int, Bitmap>
            {
                { 0, ConvertToFormat((Bitmap)Bitmap.FromFile("0.png"), PixelFormat.Format24bppRgb) },
                { 1, ConvertToFormat((Bitmap)Bitmap.FromFile("1.png"), PixelFormat.Format24bppRgb) },
                { 2, ConvertToFormat((Bitmap)Bitmap.FromFile("2.png"), PixelFormat.Format24bppRgb) },
                { 3, ConvertToFormat((Bitmap)Bitmap.FromFile("3.png"), PixelFormat.Format24bppRgb) },
                { 4, ConvertToFormat((Bitmap)Bitmap.FromFile("4.png"), PixelFormat.Format24bppRgb) },
                { 5, ConvertToFormat((Bitmap)Bitmap.FromFile("5.png"), PixelFormat.Format24bppRgb) },
                { 6, ConvertToFormat((Bitmap)Bitmap.FromFile("6.png"), PixelFormat.Format24bppRgb) },
                { 7, ConvertToFormat((Bitmap)Bitmap.FromFile("7.png"), PixelFormat.Format24bppRgb) },
                { 8, ConvertToFormat((Bitmap)Bitmap.FromFile("8.png"), PixelFormat.Format24bppRgb) },
                { 9, ConvertToFormat((Bitmap)Bitmap.FromFile("9.png"), PixelFormat.Format24bppRgb) },
            };

            var oYmaxValue = GetValueFromImage(templatesDict, oYmaxValueImage);
            var oYminValue = GetValueFromImage(templatesDict, oYminValueImage);
            var oXmaxValue = GetValueFromImage(templatesDict, oXmaxValueImage);
            var oXminValue = GetValueFromImage(templatesDict, oXminValueImage);
        }

        public static Bitmap ConvertToFormat(Bitmap image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }

        private static int GetValueFromImage(Dictionary<int, Bitmap> templatesDict, Bitmap valueImage)
        {
            var sourceImage = ConvertToFormat(valueImage, PixelFormat.Format24bppRgb);

            // create template matching algorithm's instance
            // (set similarity threshold to 98%)

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.95f);
            // find all matchings with specified above similarity

            List<DigitData> digits = new List<DigitData>();

            foreach (KeyValuePair<int, Bitmap> template in templatesDict)
            {
                System.Console.WriteLine("processing: " + template.Key);
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template.Value);
                // highlight found matchings

                BitmapData data = sourceImage.LockBits(
                     new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                     ImageLockMode.ReadWrite, sourceImage.PixelFormat);


                foreach (TemplateMatch m in matchings)
                {
                    Drawing.Rectangle(data, m.Rectangle, Color.White);

                    digits.Add(new DigitData() { X = m.Rectangle.Location.X, Y = m.Rectangle.Location.Y, Digit = template.Key });

                    //MessageBox.Show(m.Rectangle.Location.ToString());
                    System.Console.WriteLine(m.Rectangle.Location.ToString());
                    // do something else with matching
                }
                sourceImage.UnlockBits(data);
            }

            digits = digits.OrderBy(x => x.X).ToList();
            string number = "";

            foreach (var digit in digits)
            {
                number = number + digit.Digit;
            }

            return int.Parse(number);
        }

        private static Bitmap CropImage(Bitmap bmpImage, RectangleF cropArea)
        {
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private static void GetPixels(Bitmap bmp)
        {
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    if (pixel.R == 70 && pixel.G == 130 && pixel.B == 180)
                    {
                        _curvePixels.Add(new PixelData() { Color = pixel, X = x, Y = y }); 
                    }
                    if (pixel.R == 211 && pixel.G == 211 && pixel.B == 211)
                    {
                        _gridPixels.Add(new PixelData() { Color = pixel, X = x, Y = y }); 
                    }
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 0)
                    {
                        _labelPixels.Add(new PixelData() { Color = pixel, X = x, Y = y }); 
                    }
                }
            }
        }
    }
}
