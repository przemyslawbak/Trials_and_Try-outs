using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicConfig.Models
{
    public class SimpleModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string SomeLongText { get; set; }
        public List<string> ItemsList { get; set; }
    }
}
