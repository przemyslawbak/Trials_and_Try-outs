using System;
using System.Drawing;
using System.IO;
using tessnet2;

namespace ImageToText
{
    //does not work at all :(
    class Program
    {
        static void Main(string[] args)
        {
            var image = new Bitmap(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "jcaptcha.jpg"));
            var ocr = new Tesseract();
            ocr.SetVariable("tessedit_char_whitelist", "0123456789ABCDEFGHIJKLMNOPRSQTUVWXYZ"); // If digit only
                                                                                                //@"C:\OCRTest\tessdata" contains
                                                                                                //the language package, without this the method crash and app breaks
            ocr.Init(@"C:\OCRTest\tessdata", "eng", true);
            var result = ocr.DoOCR(image, Rectangle.Empty);
            foreach (Word word in result)
                Console.WriteLine("{0} : {1}", word.Confidence, word.Text);
                Console.ReadKey();
        }
    }
}
