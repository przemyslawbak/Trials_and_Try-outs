using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;

namespace BasicConfig.Controllers
{
    public class BasicController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BasicController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult JsonData()
        {
            var data = ModelHelper.MathData();
            return Json(data);
        }

        public JsonResult JsonMultiLineData()
        {
            var data = ModelHelper.MultiLineData();
            return Json(data);
        }


        public JsonResult JsonY2MultiLineData()
        {
            var data = ModelHelper.MultiLineDataForY2();
            return Json(data);
        }

        public JsonResult JsonY2Data()
        {
            var data = ModelHelper.Y2Data();
            return Json(data);
        }
    }
}
