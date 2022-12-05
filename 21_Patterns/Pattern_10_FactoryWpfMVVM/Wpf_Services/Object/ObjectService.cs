using System;

namespace Wpf_Services.Object
{
    public class ObjectService : IObjectService
    {
        public ObjectService()
        {
            Factory = new ObjectFactory();
        }

        public IObjectFactory Factory { get; set; }

        public IObjectModel GetObject(string firstName)
        {
            return Factory.GetObject(firstName);
        }
    }
}
