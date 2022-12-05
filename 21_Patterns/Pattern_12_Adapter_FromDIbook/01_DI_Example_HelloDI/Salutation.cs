using System;

namespace _01_DI_Example_HelloDI
{
    internal class Salutation
    {
        private readonly IMessageWriter writer;

        public Salutation(IMessageWriter writer)
        {
            this.writer = writer ?? throw new ArgumentNullException("writer");
        }

        public void Exclaim()
        {
            this.writer.Write("Hello DI!");
        }
    }
}