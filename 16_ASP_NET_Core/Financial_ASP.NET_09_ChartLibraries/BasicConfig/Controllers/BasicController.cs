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

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LineChart()
        {
            ViewBag.Title = "ChartJS: Line Charts";
            return View();
        }

        public IActionResult ScatterChart()
        {
            ViewBag.Title = "ChartJS: Scatter Charts";
            return View();
        }

        public IActionResult BarChart()
        {
            ViewBag.Title = "ChartJS: Bar Charts";
            return View();
        }

        public IActionResult PolarPie()
        {
            ViewBag.Title = "ChartJS: Polar and Pie Charts";
            return View();
        }


        public JsonResult JsonChartJsData(int numPoints)
        {
            var data = ModelHelper.ChartJsData(numPoints);
            return Json(data);
        }

        public JsonResult JsonIndexData()
        {
            var data = ModelHelper.CsvToIndexData();
            return Json(data);
        }
    }
}
