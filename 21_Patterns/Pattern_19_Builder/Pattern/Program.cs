using System;
using System.Collections.Generic;

namespace Pattern
{
    //Budowniczy

    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HtmlBuilder("ul");
            builder.AddChild("li", "witaj").AddChild("li", "świecie");
            Console.WriteLine(builder.ToString());

            Console.ReadKey();
        }
    }

    class HtmlElement
    {
        public string Name, Text;
        public List<HtmlElement> Elements = new List<HtmlElement>();
        private const int indentSize = 2;
        public HtmlElement() { }
        public HtmlElement(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }

    class HtmlBuilder
    {
        protected readonly string _rootName;
        protected HtmlElement root = new HtmlElement();
        public HtmlBuilder(string rootName)
        {
            _rootName = rootName;
            root.Name = rootName;
        }
        public HtmlBuilder AddChild(string childName, string childText)
        {
            var e = new HtmlElement(childName, childText);
            root.Elements.Add(e);
            return this;
        }
        public override string ToString() => root.ToString();
    }
}
