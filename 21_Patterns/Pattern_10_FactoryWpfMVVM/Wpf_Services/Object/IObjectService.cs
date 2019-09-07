using Wpf_Models;

namespace Wpf_Services.Object
{
    public interface IObjectService
    {
        ObjectModel GetObject(string firstName);
    }
}