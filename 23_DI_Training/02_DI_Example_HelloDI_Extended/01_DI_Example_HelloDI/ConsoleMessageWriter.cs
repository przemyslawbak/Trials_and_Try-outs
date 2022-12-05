namespace _01_DI_Example_HelloDI
{
    public class ConsoleMessageWriter : IMessageWriter
    {
        public void Write(string message)
        {
            System.Console.WriteLine(message);
        }
    }
}