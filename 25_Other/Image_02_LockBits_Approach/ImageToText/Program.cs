using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Path.Combine("chart.png");

            Bitmap bmp = new Bitmap(path);
            byte[] data = new byte[bmp.Height * bmp.Width * 3];
            BitmapData bmp_ = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            System.Runtime.InteropServices.Marshal.Copy(data, 0, bmp_.Scan0, bmp.Height * bmp.Width * 3);
            bmp.UnlockBits(bmp_);



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
