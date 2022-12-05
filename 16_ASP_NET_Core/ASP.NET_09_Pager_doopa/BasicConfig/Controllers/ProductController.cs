using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Controllers
{
    //dopa https://www.youtube.com/watch?v=2YXhkdA3Ils bootstrap missing?
    public class ProductController : Controller
    {
        private IPanelRepo _repo;
        public ProductController(IPanelRepo repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            IOrderedQueryable<ColorPanel> list = _repo.GetPanels();

            var model = await PagingList.CreateAsync(list, 3, page);

            return View();
        }
    }


}
