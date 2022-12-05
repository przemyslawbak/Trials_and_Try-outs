using BasicConfig.Infrastructure;
using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace BasicConfig.Controllers
{
    //ok https://www.youtube.com/watch?v=bXJOpviqXbs
    public class ProductController : Controller
    {
        private IPanelRepo _repo;
        public ProductController(IPanelRepo repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult GetImage()
        {
            DrawTextConverter textConvert = new DrawTextConverter();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, "test.png");
            MemoryStream img = textConvert.DrawText("first, create a dummy bitmap just to get a graphics object");


            using (Image srcImage = Image.FromStream(img))
            {
                //zapisanie głównego
                srcImage.Save(path, ImageFormat.Png);

                Image[] imgarray = SplitImage(srcImage);


                using (var streak = new MemoryStream())
                {
                    srcImage.Save(streak, ImageFormat.Png);
                    return File(streak.ToArray(), "image/png");
                }
            }


        }

        private Image[] SplitImage(Image srcImage)
        {
                //setup
                int width = srcImage.Width / 5;
                int height = srcImage.Height;
            Image[] imgarray = new Image[5];

            //cięcie
            for (int i = 0; i < imgarray.Length; i++)
            {
                imgarray[i] = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(imgarray[i]))
                {
                    graphics.DrawImage(srcImage, new Rectangle(0, 0, width, height), new Rectangle(i * width, 0, width, height), GraphicsUnit.Pixel);
                }
                string piecePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), i.ToString() + ".png");
                imgarray[i].Save(piecePath, ImageFormat.Png);
                // I'm not sure if the using here will work or not. It might work
                // to just remove the using block if you have issues.
            }

            return imgarray;
        }
    }


}
