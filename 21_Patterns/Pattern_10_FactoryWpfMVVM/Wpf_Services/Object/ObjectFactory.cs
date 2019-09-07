using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_Models;

namespace Wpf_Services.Object
{
    public class ObjectFactory
    {
        public ObjectModel GetObject(string firstName)
        {
            ObjectModel objectModel = new ObjectModel(firstName, new SecondNameProvider());

            return objectModel;
        }
    }
}
