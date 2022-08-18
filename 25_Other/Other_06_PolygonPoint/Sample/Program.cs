using Sample;

namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            var polygon = new PointModel[] { //Baltic Sea coordinates
                new PointModel() { X = 37.8, Y = 62.6 },
                new PointModel() { X = 19, Y = 52.7 },
                new PointModel() { X = 9.3, Y = 54.2 },
                new PointModel() { X = 10.8, Y = 58.1 },
                new PointModel() { X = 27.1, Y = 67.9 },
                new PointModel() { X = 23.2, Y = 62},
            };

            var point = new PointModel() { X = 30.8, Y = 61.7 };

            Console.WriteLine(IsPointInPolygon(point, polygon)); //true
        }

        public static bool IsPointInPolygon(PointModel p, PointModel[] polygon)
        {
            double minX = polygon[0].X;
            double maxX = polygon[0].X;
            double minY = polygon[0].Y;
            double maxY = polygon[0].Y;
            for (int i = 1; i < polygon.Length; i++)
            {
                PointModel q = polygon[i];
                minX = Math.Min(q.X, minX);
                maxX = Math.Max(q.X, maxX);
                minY = Math.Min(q.Y, minY);
                maxY = Math.Max(q.Y, maxY);
            }

            if (p.X < minX || p.X > maxX || p.Y < minY || p.Y > maxY)
            {
                return false;
            }

            // https://wrf.ecse.rpi.edu/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                if ((polygon[i].Y > p.Y) != (polygon[j].Y > p.Y) &&
                     p.X < (polygon[j].X - polygon[i].X) * (p.Y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X)
                {
                    inside = !inside;
                }
            }

            return inside;
        }
    }
}



