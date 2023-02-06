using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine("chart.png");

            var pixels = GetPixels(path);
        }

        private static List<PixelData> GetPixels(string path)
        {
            Bitmap b = new Bitmap(path);

            List<PixelData> colorList = new List<PixelData>
            {

            };

            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    colorList.Add(new PixelData() { Color = b.GetPixel(x, y), X = x, Y = y });
                }
            }

            return colorList;
        }
    }
}
