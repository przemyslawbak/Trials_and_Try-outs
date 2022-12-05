using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class SimpleViewModel
    {
        public string[] NameImages { get; set; }
        public string[] EmailImages { get; set; }
        public string[] SomeLongTextImages { get; set; }
        public List<string[]> ListImages { get; set; }
    }
}
