using LogAn;
using NSubstitute;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        [Test]
        public void Analyze_TooShortFileName_CallLogger()
        {
            //Utworzenie obiektu-makiety,
            //wzglêdem którego pod koniec
            //testu wykonamy asercjê
            ILogger logger = Substitute.For<ILogger>();
            LogAnalyzer analyzer = new LogAnalyzer(logger);
            analyzer.Analyze("a.txt"); //metoda LogAnalyzer
            //Okreœlenie oczekiwania
            //z wykorzystaniem API
            //frameworka NSub
            logger.Received().LogError("Nazwa pliku jest za krótka: a.txt");
        }
    }
}
