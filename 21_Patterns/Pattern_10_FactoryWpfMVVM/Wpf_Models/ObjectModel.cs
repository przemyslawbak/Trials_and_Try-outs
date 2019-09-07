using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Models
{
    public class ObjectModel
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }

        public ObjectModel(string firstName, ISecondNameProvider someService)
        {
            FirstName = firstName;
            SecondName = someService.SecondName;
        }
    }
}
