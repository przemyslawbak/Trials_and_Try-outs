using System.Collections.Generic;
using System.Linq;

namespace BasicConfig.Models
{
    public interface IPanelRepo
    {
        IOrderedQueryable<ColorPanel> GetPanels();
    }
}