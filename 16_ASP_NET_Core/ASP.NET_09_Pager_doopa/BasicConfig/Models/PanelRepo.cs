using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return _context.ColorPanels.OrderBy(c => c.ID);
        }
    }
}
