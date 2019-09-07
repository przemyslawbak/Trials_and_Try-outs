using System;
using System.Collections.Generic;

namespace Pattern_
{
    //factory: https://dev.to/gary_woodfine/how-to-use-factory-method-design-pattern-in-c-3ia3
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a number of wheels between 1 and 5 to build a vehicle and press enter");

            var wheels = Console.ReadLine();
            var vehicle = VehicleFactory.Build(Convert.ToInt32(wheels));
            Console.WriteLine($" You built a {vehicle.GetType().Name}");
            // Wait for user
            Console.ReadKey();
        }
    }

    public interface IVehicle
    {

    }
    public class Unicycle : IVehicle
    {

    }
    public class Car : IVehicle
    {

    }
    public class Motorbike : IVehicle
    {

    }
    public class Truck : IVehicle
    {

    }

    public static class VehicleFactory
    {
        public static IVehicle Build(int numberOfWheels)
        {
            switch (numberOfWheels)
            {
                case 1:
                    return new Unicycle();
                case 2:
                case 3:
                    return new Motorbike();
                case 4:
                    return new Car();
                default:
                    return new Truck();

            }
        }
    }

}
