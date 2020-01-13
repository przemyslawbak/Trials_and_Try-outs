using BasicConfig.Models;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Components
{
    public class Dupa : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(IOrderedQueryable<ColorPanel> list, int page = 1)
        {
            PagingList<ColorPanel> model = await PagingList.CreateAsync(list, 3, page);

            return View(model);
        }
    }
}
