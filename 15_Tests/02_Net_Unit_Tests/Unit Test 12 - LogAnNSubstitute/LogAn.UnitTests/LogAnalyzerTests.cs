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
            //wzgl�dem kt�rego pod koniec
            //testu wykonamy asercj�
            ILogger logger = Substitute.For<ILogger>();
            LogAnalyzer analyzer = new LogAnalyzer(logger);
            analyzer.Analyze("a.txt"); //metoda LogAnalyzer
            //Okre�lenie oczekiwania
            //z wykorzystaniem API
            //frameworka NSub
            logger.Received().LogError("Nazwa pliku jest za kr�tka: a.txt");
        }
    }
}
