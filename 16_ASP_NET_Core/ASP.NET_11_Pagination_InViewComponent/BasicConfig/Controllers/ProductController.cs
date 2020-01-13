using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Linq;
using System.Threading.Tasks;

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

        public IActionResult Index(int page = 1)
        {
            IOrderedQueryable<ColorPanel> list = _repo.GetPanels();

            ViewBag.TableList = list;
            ViewBag.PageNo = page;

            return View();
        }
    }


}
