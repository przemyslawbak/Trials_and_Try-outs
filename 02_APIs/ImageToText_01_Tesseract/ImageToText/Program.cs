using Patagames.Ocr;
using Patagames.Ocr.Enums;
using System;
using System.IO;

namespace ImageToText
{
    class Program
    {
        static void Main(string[] args)
        {
            Program obj = new Program();
            obj.ConvertImageToText();
        }

        //Poor results :(
        public void ConvertImageToText()
        {
            using (var api = OcrApi.Create())
            {
                api.Init(Languages.English);
                string image = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "jcaptcha.jpg");
                string plainText = api.GetTextFromImage(image);
                Console.WriteLine(plainText);
                Console.Read();
            }
        }
    }
}
