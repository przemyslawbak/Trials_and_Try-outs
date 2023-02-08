using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageToText
{
    class Program
    {
        private static List<PixelData> _curvePixels = new List<PixelData>();
        private static List<PixelData> _gridPixels = new List<PixelData>();
        private static List<PixelData> _labelsPixels = new List<PixelData>();
        static void Main(string[] args)
        {
            string path = Path.Combine("chart.png");

            GetPixels(path);

            _curvePixels = _curvePixels
                .GroupBy(x => x.X)
                .Select(g => new PixelData { Color = g.FirstOrDefault().Color, X = g.FirstOrDefault().X, Y = (int)g.Average(x => x.Y) })
                .ToList();

            var maxY = _gridPixels.Where(x => x.X == 50).Select(x => x.Y).Max();
            var minY = _gridPixels.Where(x => x.X == 50).Select(x => x.Y).Min();
            var maxYlabel = maxY;
            var minYlabel = minY;
            var maxX = _gridPixels.Where(y => y.Y == maxY).Select(y => y.X).Max();
            var minX = _gridPixels.Where(y => y.Y == maxY).Select(y => y.X).Min();
            var maxXlabel = _gridPixels.Where(y => y.Y == maxY + 2).Select(y => y.X).Max();
            var minXlabel = _gridPixels.Where(y => y.Y == maxY + 2).Select(y => y.X).Min();

            //todo: get number values
            //todo: assign values to Y ranges
            //todo: parse grid pixels with number values
            //todo: 
        }

        private static void GetPixels(string path)
        {
            Bitmap bmp = new Bitmap(path);

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
                        _labelsPixels.Add(new PixelData() { Color = pixel, X = x, Y = y }); 
                    }
                }
            }
        }
    }
}
