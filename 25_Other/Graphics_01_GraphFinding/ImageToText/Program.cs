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
            var oXlabelsYmin = 0;
            var oXlabelsYmax = minYgraph;
            var oYlabelHeight = 20;
            var oXlabelHeight = 20;
            var oXlabelWidth = 60;

            var oYmaxValue = 0;
            var oYminValue = 0;
            var oXmaxValue = 0;
            var oXminValue = 0;

            //todo: get number values
            //todo: assign values to Y ranges
            //todo: parse grid pixels with number values
            //todo: 
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
