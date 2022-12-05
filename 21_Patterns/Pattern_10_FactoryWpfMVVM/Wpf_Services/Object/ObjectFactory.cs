using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Services.Object
{
    public class ObjectFactory : IObjectFactory
    {
        public IObjectModel GetObject(string firstName)
        {
            return new ObjectModel(firstName, new SomeService());
        }
    }
}
