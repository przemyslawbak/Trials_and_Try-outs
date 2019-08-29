using Wpf_Models;

namespace Wpf_Services.Building
{
    public interface IMediumCarBuilder : IBuilderService
    {
        CarProductModel GetCar();
    }
}