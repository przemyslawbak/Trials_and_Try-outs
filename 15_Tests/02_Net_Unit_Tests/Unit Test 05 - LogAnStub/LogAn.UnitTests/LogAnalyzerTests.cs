using LogAn;
using NUnit.Framework;
using System;
using static LogAn.LogAnalyzer;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        [Test]
        public void IsValidFileName_NameSupportedExtension_ReturnsTrue()
        {
            FakeExtensionManager myFakeManager = new FakeExtensionManager(); //Przes³anie namiastki
            myFakeManager.WillBeValid = true; //konfiguracja namiastki która zwraca true
            LogAnalyzer log = new LogAnalyzer(myFakeManager);
            bool result = log.IsValidLogFileNameee("short.ext");
            Assert.True(result);
        }
    }

    //Prosty kod namiastki
    internal class FakeExtensionManager : IExtensionManager //Definicja namiastki z wykorzystaniem najprostszego mo¿liwego mechanizmu
    {
        public bool WillBeValid = false;
        public bool IsValid(string fileName)
        {
            return WillBeValid;
        }
    }
}