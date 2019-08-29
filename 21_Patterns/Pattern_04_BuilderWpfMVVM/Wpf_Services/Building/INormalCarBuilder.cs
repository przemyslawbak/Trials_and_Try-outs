using Wpf_Models;

namespace Wpf_Services.Building
{
    public interface INormalCarBuilder : IBuilderService
    {
        CarProductModel GetCar();
    }
}