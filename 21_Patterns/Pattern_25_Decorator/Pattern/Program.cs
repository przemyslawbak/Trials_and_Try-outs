using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Pattern
{
    //Dekorator

    class Program
    {
        static void Main(string[] args)
        {
            //
        }
    }

    public class CodeBuilder //dekorator
    {
        private StringBuilder builder = new StringBuilder();
        private int indentLevel = 0;
        public CodeBuilder Indent()
        {
            indentLevel++;
            return this;
        }

        public StringBuilder Append(string value)
        {
            return builder.Append(value);
        }
        public StringBuilder AppendLine()
        {
            return builder.AppendLine();
        }
    }
}
