using BasicConfig.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Threading.Tasks;

namespace BasicConfig.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<PartialViewResult> LoadSomeShit()
        {
            SimpleModel model = new SimpleModel()
            {
                Email = "ddd@ddd.pl",
                Name = "Dddddddddddd",
                SomeLongText = "ddddddddddddddddddddddddddddddd"
            };
            await Task.Delay(5000);
            ViewBag.Dupa = "viewbag dupa displayed, together with partial";

            return new PartialViewResult
            {
                ViewName = "_LoadSomeShit",
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                }
            };
        }
    }


}
