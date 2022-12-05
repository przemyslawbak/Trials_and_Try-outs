using BasicConfig.Infrastructure;
using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
            SimpleModel model = new SimpleModel()
            {
                Name = "Przemyslaw Bak",
                Email = "przemyslaw.pszemek@wp.pl",
                SomeLongText = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua",
                ItemsList = new List<string>() { "Chair", "Pen", "Cellphone", "Handmade something" }
            };

            SimpleViewModel vm = new SimpleViewModel()
            {
                NameImages = ConvertToMultipleImages(model.Name),
                EmailImages = ConvertToMultipleImages(model.Email),
                SomeLongTextImages = ConvertToMultipleImages(model.SomeLongText),
                ListImages = GetListImages(model.ItemsList)
            };

            return View(vm);
        }

        private List<string[]> GetListImages(List<string> itemsList)
        {
            List<string[]> list = new List<string[]>();
            foreach (var item in itemsList)
            {
                list.Add(ConvertToMultipleImages(item));
            }
            return list;
        }

        private string[] ConvertToMultipleImages(string text)
        {
            DrawTextConverter textConvert = new DrawTextConverter();
            MemoryStream imageStream = textConvert.DrawText(text);

            using (Image srcImage = Image.FromStream(imageStream))
            {
                return ConvertImageToString(SplitImage(srcImage));
            }


        }

        private string[] ConvertImageToString(Image[] imgarray)
        {
            string[] array = new string[5];

            for (int i = 0; i < imgarray.Length; i++)
            {
                using (var ms = new MemoryStream())
                {
                    imgarray[i].Save(ms, ImageFormat.Png);
                    byte[] byteData =  ms.ToArray();
                    string imreBase64Data = Convert.ToBase64String(byteData);
                    array[i] = string.Format("data:image/png;base64,{0}", imreBase64Data);
                }
            }

            return array;
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
            }

            return imgarray;
        }
    }


}
