using System;
using Wpf_Models;

namespace Wpf_Services.Building
{
    public class NormalCarBuilder : INormalCarBuilder, IBuilderService
    {
        private CarProductModel myCar = new CarProductModel();

        public void CarType()
        {
            myCar.Model = CarModel.Economy;
        }

        public CarProductModel GetCar()
        {
            return myCar;
        }

        public void ProvideACType()
        {
            myCar.AirConditioning = AirConditioning.Manual;
        }

        public void ProvideColorType()
        {
            myCar.Color = Color.Normal;
        }

        public void ProvideUpholsteryType()
        {
            myCar.Upholstery = Upholstery.Normal;
        }

        public void ProvideWheelType()
        {
            myCar.WheelType = Wheels.Metal;
        }
    }
}
