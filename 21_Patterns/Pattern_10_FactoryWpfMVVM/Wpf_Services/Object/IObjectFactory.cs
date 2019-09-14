using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf_Services.Object
{
    public interface IObjectFactory
    {
        IObjectModel GetObject(string firstName);
    }
}
