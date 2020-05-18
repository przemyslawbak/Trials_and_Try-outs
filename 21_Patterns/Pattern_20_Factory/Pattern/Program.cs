using System;
using System.Collections.Generic;

namespace Pattern
{
    //Fabryka

    class Program
    {
        static void Main(string[] args)
        {
            var myPoint = PointFactory.NewCartesianPoint(3, 4);
        }
    }

    public enum CoordinateSystem
    {
        Cartesian,
        Polar
    }

    public class Point
    {
        private double x;
        private double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }
        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(rho * Math.Cos(theta), rho * Math.Sin(theta));
        }

    }

    class PointFactory
    {
        public static Point NewCartesianPoint(float x, float y)
        {
            return new Point(x, y); //musi być publiczny
        }
        public static Point NewPolarPoint(float x, float y)
        {
            return new Point(x, y); //musi być publiczny
        }
    }
}
