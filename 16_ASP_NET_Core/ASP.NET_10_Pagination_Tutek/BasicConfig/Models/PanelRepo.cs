using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace BasicConfig.Models
{
    public class PanelRepo : IPanelRepo
    {
        private ApplicationDbContext _context;

        public PanelRepo(ApplicationDbContext ctx)
        {
            _context = ctx;
        }

        public IOrderedQueryable<ColorPanel> GetPanels()
        {
            return _context.ColorPanels.AsNoTracking().OrderBy(c => c.ID);
        }
    }
}
