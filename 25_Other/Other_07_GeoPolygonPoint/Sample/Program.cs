using Sample;

namespace Activator
{
    //https://stackoverflow.com/a/7199522
    //wrong conversion from JAVA?

    class Program
    {
        public static double pi = 3.14159265;
        public static double twopi = 2 * pi;

        static void Main(string[] args)
        {
            var polygon = new PointModel[] {
                new PointModel() { X = -11, Y = 11 },
                new PointModel() { X = 10, Y = 10 },
                new PointModel() { X = 9, Y = -9 },
                new PointModel() { X = -8, Y = -8 },
            };

            var latArray = polygon.Select(p => p.Y).ToArray();
            var lonArray = polygon.Select(p => p.X).ToArray();

            var point = new PointModel() { Y = 4, X = 4 };

            Console.WriteLine(IsPointInPolygon(point.Y, point.X, latArray, lonArray));
        }

        private static bool IsPointInPolygon(double latitude, double longitude, double[] lat_array, double[] long_array)
        {
            double angle = 0;
            double point1_lat;
            double point1_long;
            double point2_lat;
            double point2_long;
            int n = lat_array.Length;

            for (int i = 0; i < n; i++) //exception
            {
                point1_lat = lat_array[i] - latitude;
                point1_long = long_array[i] - longitude;
                point2_lat = (lat_array[i + 1] % n) - latitude;
                point2_long = (long_array[i + 1] % n) - longitude;

                angle += Angle2D(point1_lat, point1_long, point2_lat, point2_long);
            }

            if (Math.Abs(angle) < pi)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static double Angle2D(double y1, double x1, double y2, double x2)
        {
            double theta2;
            double dtheta;
            double theta1;
            theta1 = Math.Atan2(y1, x1);
            theta2 = Math.Atan2(y2, x2);
            dtheta = theta2 - theta1;
            while (dtheta > pi)
            {
                dtheta -= twopi;
            }

            while (dtheta < (pi * -1))
            {
                dtheta += twopi;
            }

            return dtheta;
        }
    }
}



