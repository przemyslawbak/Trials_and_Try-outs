using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf_Models
{
    //https://www.dotnetforall.com/builder-pattern-different-versions/
    public enum CarModel
    {
        Economy,
        Medium,
        Luxury
    }

    public enum Wheels
    {
        Alloy,
        Metal,
        Polished
    }

    public enum Upholstery
    {
        Normal,
        RayonFabric,
        Leather
    }

    public enum Color
    {
        Normal,
        Metallic,
        Glossy
    }

    public enum AirConditioning
    {
        Manual,
        Automatic,
        ClimateControl
    }
    public class CarProductModel
    {
        public CarModel Model { get; set; }
        public Wheels WheelType { get; set; }
        public Upholstery Upholstery { get; set; }
        public Color Color { get; set; }
        public AirConditioning AirConditioning { get; set; }

        public void Display()
        {
            MessageBox.Show(string.Format(@"Model: " + Model + "; " +
                                            "WheelType: " + WheelType + "; " +
                                            "Upholstery: " + Upholstery + "; " +
                                            "Color: " + Color + "; " +
                                            "AirCon: " + AirConditioning));
        }
    }
}
