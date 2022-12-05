using Wpf_Models;

namespace Wpf_Services.Building
{
    public interface IBuilderService
    {
        void CarType();
        void ProvideACType();
        void ProvideColorType();
        void ProvideUpholsteryType();
        void ProvideWheelType();
        CarProductModel GetCar();
    }
}