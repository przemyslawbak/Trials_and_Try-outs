using Sample;

namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            var polygon = new PointModel[] {
                new PointModel() { X = 1, Y = 1 },
                new PointModel() { X = 1, Y = 10 },
                new PointModel() { X = 10, Y = 10 },
                new PointModel() { X = 12, Y = 5 },
                new PointModel() { X = 10, Y = 1 },
            };

            var point = new PointModel() { X = 11.9, Y = 5 };

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



