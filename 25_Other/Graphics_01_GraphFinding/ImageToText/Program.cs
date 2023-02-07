using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine("chart.png");

            var curvePixels = GetPixels(path, 70, 130, 180);
            var gridPixels = GetPixels(path, 211, 211, 211);
            var numbersPixels = GetPixels(path, 0, 0, 0);

            curvePixels = curvePixels
                .GroupBy(x => x.X)
                .Select(g => new PixelData { Color = g.FirstOrDefault().Color, X = g.FirstOrDefault().X, Y = (int)g.Average(x => x.Y) })
                .ToList();

            //todo: get number values
            //todo: assign values to Y ranges
            //todo: parse grid pixels with number values
            //todo: 
        }

        private static List<PixelData> GetPixels(string path, int r, int g, int b)
        {
            Bitmap bmp = new Bitmap(path);

            List<PixelData> curvePixels = new List<PixelData>();

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.GetPixel(x, y);
                    if (pixel.R == r && pixel.G == g && pixel.B == b)
                    {
                        curvePixels.Add(new PixelData() { Color = pixel, X = x, Y = y }); 
                    }
                }
            }

            return curvePixels;
        }
    }
}
