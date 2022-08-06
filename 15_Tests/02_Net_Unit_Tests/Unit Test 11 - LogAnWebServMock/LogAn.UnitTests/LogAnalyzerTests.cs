using LogAn;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        [Test]
        public void Analyze_TooShortFileName_CallsWebService()
        {
            FakeWebService mockService = new FakeWebService();
            LogAnalyzer log = new LogAnalyzer(mockService);
            string tooShortFileName = "abc.ext";
            log.Analyze(tooShortFileName);
            //Zauwa�my, �e asercja dotyczy obiektu-makiety, a nie klasy LogAnalyzer.
            //To dlatego, �e testujemy interakcje mi�dzy klas� LogAnalyzer a us�ug� sieciow�.
            StringAssert.Contains("Nazwa pliku jest zbyt kr�tka:abc.ext", mockService.LastError);
        }
    }
    //Na pierwszy rzut oka wygl�da on jak
    //namiastka, ale zawiera fragment kodu, kt�ry sprawia, �e jest obiektem-makiet�.
    public class FakeWebService : IWebService
    {
        public string LastError;
        public void LogError(string message)
        {
            LastError = message;
        }
    }
}