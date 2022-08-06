using LogAn;
using NUnit.Framework;
using System;
using static LogAn.LogAnalyzer;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        //Aby ten test przeszed³, musielibyœmy napisaæ kod, który wywo³uje mened¿era rozszerzeñ
        //z klauzul¹ try-catch i zwraca false, jeœli sterowanie trafi do klauzuli catch.
        [Test]
        public void IsValidFileName_ExtManagerThrowsException_ReturnsFalse()
        {
            FakeExtensionManager myFakeManager = new FakeExtensionManager();
            myFakeManager.WillThrow = new Exception("to jest sztuczny wyj¹tek");
            LogAnalyzer log = new LogAnalyzer(myFakeManager);
            bool result = log.IsValidLogFileNameee("anything.anyextension");
            Assert.False(result);
        }
    }
    //kod namiastki
    internal class FakeExtensionManager : IExtensionManager
    {
        public bool WillBeValid = false;
        public Exception WillThrow = null;
        public bool IsValid(string fileName)
        {
            if (WillThrow != null)
            { throw WillThrow; }
            return WillBeValid;
        }
    }
}