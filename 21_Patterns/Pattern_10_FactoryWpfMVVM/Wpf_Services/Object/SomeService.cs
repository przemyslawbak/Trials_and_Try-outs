using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Services.Object
{
    public class SomeService : ISomeService
    {
        public SomeService()
        {
            SecondName = "SecondService";
        }

        public string SecondName { get; set; }
    }
}
