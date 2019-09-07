using System;

namespace Pattern_
{
    //factory: https://dev.to/gary_woodfine/how-to-use-factory-method-design-pattern-in-c-3ia3
    class Program
    {
        static void Main(string[] args)
        {
            ICarStandardFactory factory = new CarStandardFactory();

            CarStandard standard = factory.GetNewCar();

            // Wait for user
            Console.ReadKey();
        }
    }

    public class CarStandard
    {
        public CarStandard(ILights lights, IWheels wheels)
        {
            Console.WriteLine("wheels: " + wheels.Size.ToString() + ", " + wheels.Tires);
            Console.WriteLine("lights: " + lights.Fog.ToString() + ", " + lights.Type);
        }
    }

    public class CarStandardFactory : ICarStandardFactory
    {
        public CarStandard GetNewCar()
        {
            CarStandard standard = new CarStandard(new Lights(), new Wheels());

            return standard;
        }
    }

    public interface ICarStandardFactory
    {
        CarStandard GetNewCar();
    }

    public class Wheels : IWheels
    {
        public int Size { get; set; }
        public string Tires { get; set; }

        public Wheels()
        {
            Size = 19;
            Tires = "Pirelli";
        }
    }

    public interface IWheels
    {
        int Size { get; set; }
        string Tires { get; set; }
    }

    public class Lights : ILights
    {
        public string Type { get; set; }
        public bool Fog { get; set; }

        public Lights()
        {
            Type = "Xenon";
            Fog = true;
        }
    }

    public interface ILights
    {
        string Type { get; set; }
        bool Fog { get; set; }
    }
}
