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

        public ActionResult GetCustIdBarCode()
        {
            DrawTextConverter textConvert = new DrawTextConverter();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, "test.png");
            MemoryStream img = textConvert.DrawText("Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua");


            using (Image srcImage = Image.FromStream(img))
            {
                // I'm not sure if the using here will work or not. It might work
                // to just remove the using block if you have issues.
                srcImage.Save(path, ImageFormat.Png);
                using (var streak = new MemoryStream())
                {
                    srcImage.Save(streak, ImageFormat.Png);
                    return File(streak.ToArray(), "image/png");
                }
            }


        }
    }


}
