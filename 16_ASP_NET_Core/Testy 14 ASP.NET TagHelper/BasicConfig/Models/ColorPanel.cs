using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class ColorPanel
    {
        public int ID { get; set; }
        public string Element1BackgroundColor { get; set; }
        public string Element2BackgroundColor { get; set; }
        public string Element3BackgroundColor { get; set; }
    }
}
