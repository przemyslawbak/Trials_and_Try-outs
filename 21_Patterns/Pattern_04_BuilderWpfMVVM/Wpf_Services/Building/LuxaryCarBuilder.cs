using Wpf_Models;

namespace Wpf_Services.Building
{
    public class LuxaryCarBuilder : ILuxaryCarBuilder, IBuilderService
    {
        private CarProductModel myCar = new CarProductModel();

        public void CarType()
        {
            myCar.Model = CarModel.Luxury;
        }

        public CarProductModel GetCar()
        {
            return myCar;
        }

        public void ProvideACType()
        {
            myCar.AirConditioning = AirConditioning.ClimateControl;
        }

        public void ProvideColorType()
        {
            myCar.Color = Color.Glossy;
        }

        public void ProvideUpholsteryType()
        {
            myCar.Upholstery = Upholstery.RayonFabric;
        }

        public void ProvideWheelType()
        {
            myCar.WheelType = Wheels.Polished;
        }
    }
}
