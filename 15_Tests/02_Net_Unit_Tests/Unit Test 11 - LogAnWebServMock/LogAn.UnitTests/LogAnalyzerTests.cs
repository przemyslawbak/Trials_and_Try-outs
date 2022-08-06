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
            //Zauwa¿my, ¿e asercja dotyczy obiektu-makiety, a nie klasy LogAnalyzer.
            //To dlatego, ¿e testujemy interakcje miêdzy klas¹ LogAnalyzer a us³ug¹ sieciow¹.
            StringAssert.Contains("Nazwa pliku jest zbyt krótka:abc.ext", mockService.LastError);
        }
    }
    //Na pierwszy rzut oka wygl¹da on jak
    //namiastka, ale zawiera fragment kodu, który sprawia, ¿e jest obiektem-makiet¹.
    public class FakeWebService : IWebService
    {
        public string LastError;
        public void LogError(string message)
        {
            LastError = message;
        }
    }
}