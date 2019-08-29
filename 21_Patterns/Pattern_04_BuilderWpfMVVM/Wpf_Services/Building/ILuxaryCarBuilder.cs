using Wpf_Models;

namespace Wpf_Services.Building
{
    public interface ILuxaryCarBuilder : IBuilderService
    {
        CarProductModel GetCar();
    }
}