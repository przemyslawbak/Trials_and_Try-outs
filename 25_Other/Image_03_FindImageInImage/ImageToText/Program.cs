using AForge.Imaging;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {
            Bitmap sourceImage = (Bitmap)Bitmap.FromFile("number.png");

            // convert to Format24bppRgb https://stackoverflow.com/a/26967840/12603542

            sourceImage = ConvertToFormat(sourceImage, PixelFormat.Format24bppRgb);

            var templatesDict = new Dictionary<int, Bitmap>
            {
                { 0, ConvertToFormat((Bitmap)Bitmap.FromFile("0.png"), PixelFormat.Format24bppRgb) },
                { 1, ConvertToFormat((Bitmap)Bitmap.FromFile("1.png"), PixelFormat.Format24bppRgb) },
                { 5, ConvertToFormat((Bitmap)Bitmap.FromFile("5.png"), PixelFormat.Format24bppRgb) },
            };

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

            int result = int.Parse(number);
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
    }
}
