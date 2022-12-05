using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf_Models;

namespace Wpf_Services.Building
{
    public class MediumCarBuilder : IMediumCarBuilder, IBuilderService
    {
        private CarProductModel myCar = new CarProductModel();

        public void CarType()
        {
            myCar.Model = CarModel.Medium;
        }

        public CarProductModel GetCar()
        {
            return myCar;
        }

        public void ProvideACType()
        {
            myCar.AirConditioning = AirConditioning.Automatic;
        }

        public void ProvideColorType()
        {
            myCar.Color = Color.Metallic;
        }

        public void ProvideUpholsteryType()
        {
            myCar.Upholstery = Upholstery.Leather;
        }

        public void ProvideWheelType()
        {
            myCar.WheelType = Wheels.Alloy;
        }
    }
}
