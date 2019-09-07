using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_Models;

namespace Wpf_Services.Object
{
    public class ObjectService : IObjectService
    {
        public ObjectModel GetObject(string firstName)
        {
            ObjectFactory factory = new ObjectFactory();

            return factory.GetObject(firstName);
        }
    }
}
