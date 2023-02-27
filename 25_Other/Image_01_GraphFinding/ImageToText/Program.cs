using AForge.Imaging;
using System;
using System.Collections;
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

            var topGridHorizontalLine = _gridPixels.Where(x => x.X == 66).Select(x => x.Y).Min();
            var bottomGridHorizontalLine = _gridPixels.Where(x => x.X == 66).Select(x => x.Y).Max();
            var leftGridVerticalLine = _gridPixels.Where(y => y.Y == topGridHorizontalLine + 2).Select(y => y.X).Min();
            var rightGridVerticalLine = _gridPixels.Where(y => y.Y == topGridHorizontalLine + 2).Select(y => y.X).Max();
            var maxYgraph = _gridPixels.Where(x => x.X == 66).Select(x => x.Y).Min();
            var minYgraph = _gridPixels.Where(x => x.X == 66).Select(x => x.Y).Max();
            var maxXgraph = _gridPixels.Where(y => y.Y == maxYgraph).Select(y => y.X).Max() - 10;
            var minXgraph = _gridPixels.Where(y => y.Y == maxYgraph).Select(y => y.X).Min() + 10;

            var oYlabelsXmin = maxXgraph + 1;
            var oYlabelsXmax = bmp.Width;
            var oXlabelsYmax = minYgraph;
            var oYlabelHeight = 24;
            var oXlabelHeight = 24;
            var oXlabelWidth = 80;

            var oYmaxRect = new RectangleF(oYlabelsXmin, topGridHorizontalLine - oYlabelHeight / 2, oYlabelsXmax - oYlabelsXmin, oYlabelHeight);
            var oYminRect = new RectangleF(oYlabelsXmin, bottomGridHorizontalLine - oYlabelHeight / 2, oYlabelsXmax - oYlabelsXmin, oYlabelHeight);
            var oXmaxRect = new RectangleF(rightGridVerticalLine - oXlabelWidth / 2, oXlabelsYmax + 16, oXlabelWidth, oXlabelHeight);
            var oXminRect = new RectangleF(leftGridVerticalLine - oXlabelWidth / 2, oXlabelsYmax + 16, oXlabelWidth, oXlabelHeight);

            var oYmaxValueImage = CropImage(bmp, oYmaxRect);
            var oYminValueImage = CropImage(bmp, oYminRect);
            var oXmaxValueImage = CropImage(bmp, oXmaxRect);
            var oXminValueImage = CropImage(bmp, oXminRect);
            oYmaxValueImage.Save("oYmaxValueImage.png", ImageFormat.Png);
            oYminValueImage.Save("oYminValueImage.png", ImageFormat.Png);
            oXmaxValueImage.Save("oXmaxValueImage.png", ImageFormat.Png);
            oXminValueImage.Save("oXminValueImage.png", ImageFormat.Png);

            var templatesDict = new Dictionary<int, Bitmap>
            {
                { -1, ConvertToFormat((Bitmap)Bitmap.FromFile("minus.png"), PixelFormat.Format24bppRgb) },
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

            var yAxisLength = bottomGridHorizontalLine - topGridHorizontalLine;
            var xAxisLength = maxXgraph - minXgraph;

            var oneYearXunitPixels = (decimal)((rightGridVerticalLine - leftGridVerticalLine) / (oXmaxValue - oXminValue));
            var oneMonthXunitPixels = (decimal)(oneYearXunitPixels / 12);

            var dataValues = ComputeDataValues(oneMonthXunitPixels, oneYearXunitPixels, maxYgraph, minYgraph, maxXgraph, minXgraph, oYmaxValue, oYminValue, oXmaxValue, oXminValue, leftGridVerticalLine);
        }

        private static List<DataModel> ComputeDataValues(
            decimal oneMonthXunitPixels, 
            decimal oneYearXunitPixels, 
            int maxYgraph, 
            int minYgraph, 
            int maxXgraph, 
            int minXgraph, 
            int oYmaxValue, 
            int oYminValue, 
            int oXmaxValue, 
            int oXminValue, 
            int leftGridVerticalLine)
        {
            var positionXstart = leftGridVerticalLine - ((int)oneYearXunitPixels * 2); //start position
            var positionXdec = (decimal)positionXstart;
            var result = new List<DataModel>();
            var months = 12;

            var startoXminValue = (int)(oXminValue - 2);

            var yearCounter = 0;
            for (int i = startoXminValue; i <= oXmaxValue; i++)
            {
                for (int j = 1; j <= months; j++)
                {
                    positionXdec = positionXdec + oneMonthXunitPixels;
                    var positionX = (int)Math.Round(positionXdec, 0, MidpointRounding.AwayFromZero);
                    if ((positionX >= minXgraph) && (positionX <= maxXgraph))
                    {
                        int? positionY = _curvePixels.Where(x => x.X == positionX).Select(x => x.Y).FirstOrDefault();

                        if (positionY.HasValue)
                        {
                            var dataValue = InterpolateValue(positionY.Value, maxYgraph, minYgraph, oYmaxValue, oYminValue);
                            result.Add(new DataModel() { DateTime = new DateTime(i, j, 28), DataValue = dataValue, X = positionX, Y = positionY.Value });
                        }
                    }
                }

                yearCounter++;
                positionXdec = (decimal)(positionXstart + (yearCounter * oneYearXunitPixels));
            }

            return result;
        }

        private static decimal InterpolateValue(int positionY, int maxYgraph, int minYgraph, int oYmaxValue, int oYminValue)
        {
            decimal result = (decimal)0.0;

            var yValueDiff = oYminValue - oYmaxValue;
            var yGraphDiff = minYgraph - maxYgraph;
            var yPixelValue = (decimal)yValueDiff / (decimal)yGraphDiff;
            var yPositionDiff = minYgraph - positionY;
            var valueDiff = yPositionDiff * yPixelValue;

            result = oYminValue - valueDiff;

            return result;
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

            ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.95f);

            List<DigitData> digits = new List<DigitData>();

            foreach (KeyValuePair<int, Bitmap> template in templatesDict)
            {
                Console.WriteLine("processing: " + template.Key);
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template.Value);

                BitmapData data = sourceImage.LockBits(
                     new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                     ImageLockMode.ReadWrite, sourceImage.PixelFormat);


                foreach (TemplateMatch m in matchings)
                {
                    Drawing.Rectangle(data, m.Rectangle, Color.White);

                    var digitData = new DigitData() { X = m.Rectangle.Location.X, Y = m.Rectangle.Location.Y, Digit = template.Key };

                    digits.Add(digitData);
                }
                sourceImage.UnlockBits(data);
            }

            digits = digits.OrderBy(x => x.X).ToList();
            string number = "";

            foreach (var digit in digits)
            {
                if (digit.Digit != -1)
                {
                    number = number + digit.Digit;
                }
            }

            var res = int.Parse(number);
            if (digits.Any(x => x.Digit == -1))
            {
                res = res * -1;
            }
            return res;
        }

        private static Bitmap CropImage(Bitmap bmpImage, RectangleF cropArea)
        {
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private static void GetPixels(Bitmap bmp)
        {
            _curvePixels = new List<PixelData>();
            _gridPixels = new List<PixelData>();
            _labelPixels = new List<PixelData>();

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    if (pixel.R == 70 && pixel.G == 130 && pixel.B == 180)
                    {
                        _curvePixels.Add(new PixelData() { Color = pixel, X = x, Y = y });
                    }
                    if (pixel.G == 211 && pixel.B == 211)
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
