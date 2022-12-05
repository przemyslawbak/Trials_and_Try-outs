using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pattern
{
    //Adapter

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }

    public class LineToPointAdapter : Collection<Point> //adapter
    {
        private static int count = 0;
        public LineToPointAdapter(Line line)
        {
            Console.WriteLine($"{++count}: Generowanie punktów dla linii" + $" [{line.Start.X},{line.Start.Y}]-" + $"[{line.End.X},{line.End.Y}] (bez buforowania)");
            int left = Math.Min(line.Start.X, line.End.X);
            int right = Math.Max(line.Start.X, line.End.X);
            int top = Math.Min(line.Start.Y, line.End.Y);

            int bottom = Math.Max(line.Start.Y, line.End.Y);
            if (right - left == 0)
            {
                for (int y = top; y <= bottom; ++y)
                {
                    Add(new Point(left, y));
                }
            }
            else if (line.End.Y - line.Start.Y == 0)
            {
                for (int x = left; x <= right; ++x)
                {
                    Add(new Point(x, top));
                }
            }
        }

        internal void ForEach(object drawPoint)
        {
            throw new NotImplementedException();
        }
    }

    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        //inne składowe pominięto
    }
    public class Line
    {
        public Point Start, End;
        private Point point1;
        private Point point2;

        public Line(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }
        //inne składowe pominięto
    }

    public class VectorRectangle : VectorObject
    {
        public VectorRectangle(int x, int y, int width, int height)
        {
            Add(new Line(new Point(x, y), new Point(x + width, y)));
            Add(new Line(new Point(x + width, y), new Point(x + width, y + height)));
            Add(new Line(new Point(x, y), new Point(x, y + height)));
            Add(new Line(new Point(x, y + height), new Point(x + width, y + height)));
        }
    }

    public class VectorObject
    {
        public void Add(Line line)
        {
            throw new NotImplementedException();
        }
    }
}
