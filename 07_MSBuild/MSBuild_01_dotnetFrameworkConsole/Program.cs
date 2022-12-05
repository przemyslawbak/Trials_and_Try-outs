using System;
using log4net;
using log4net.Config;

namespace First_Sample
{
    internal class Program
    {
        private static readonly ILog _logger = LogManager.GetLogger("main");

        private static void Main()
        {
            XmlConfigurator.Configure();

            _logger.Debug("test message from example app");

            _logger.Info("This is an info message");

            _logger.Error("Oops, here's an error");
			
			try
			{
				string s = "s";
				int a = int.Parse(s);
			}
			catch
			{
				_logger.Error("parsing test error");
			}

            Console.ReadKey();
        }
    }
}