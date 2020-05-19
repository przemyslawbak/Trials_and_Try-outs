using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Pattern
{
    //Most

    class Program
    {
        static void Main(string[] args)
        {
            var vector = new VectorRenderer();
            var circle = new Circle(vector, 5);
            circle.Draw(); //Rysowanie okręgu o promieniu 5
            circle.Resize(2);
            circle.Draw(); //Rysowanie okręgu o promieniu 10
        }
    }

    public interface IRenderer
    {
        void RenderCircle(float radius);
        // RenderSquare, RenderTriangle etc.
    }

    public class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius)
        {
            Console.WriteLine($"Rysowanie okręgu o promieniu {radius}");
        }
    }

    public abstract class Shape
    {
        protected IRenderer renderer;
        //most między rysowaną figurą a
        //komponentem, który go rysuje
        public Shape(IRenderer renderer)
        {
            this.renderer = renderer;
        }
        public abstract void Draw();
        public abstract void Resize(float factor);
    }

    public class Circle : Shape
    {
        private float radius;
        public Circle(IRenderer renderer, float radius) : base(renderer)
        {
            this.radius = radius;
        }
        public override void Draw()
        {
            renderer.RenderCircle(radius);
        }
        public override void Resize(float factor)
        {
            radius *= factor;
        }
    }
}
