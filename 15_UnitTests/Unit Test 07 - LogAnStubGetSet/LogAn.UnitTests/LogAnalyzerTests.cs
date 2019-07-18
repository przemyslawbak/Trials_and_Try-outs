using LogAn;
using NUnit.Framework;
using System;
using static LogAn.LogAnalyzer;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        //Aby ten test przeszed�, musieliby�my napisa� kod, kt�ry wywo�uje mened�era rozszerze�
        //z klauzul� try-catch i zwraca false, je�li sterowanie trafi do klauzuli catch.
        [Test]
        public void IsValidFileName_SupportedExtension_ReturnsTrue()
        {

            //stworzenie analizatora i wstrzykni�cie namiastki
            LogAnalyzer log = new LogAnalyzer();
            log.ExtensionManager = someFakeManagerCreatedEarlier; //namiastka
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