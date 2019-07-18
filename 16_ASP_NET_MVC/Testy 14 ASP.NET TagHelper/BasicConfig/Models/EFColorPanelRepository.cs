using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class EFColorPanelRepository : IColorPanelRepository
    {
        private ApplicationDbContext context;
        public EFColorPanelRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<ColorPanel> ColorPanels => context.ColorPanels;
    }
}
