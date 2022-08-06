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
        public void IsValidFileName_SupportedExtension_ReturnsTrue()
        {
            FakeExtensionManager myFakeManager = new FakeExtensionManager();
            //stworzenie analizatora i wstrzykniêcie namiastki
            LogAnalyzer log = new LogAnalyzer();
            ExtensionManagerFactory manager = new ExtensionManagerFactory();
            manager.SetManager(myFakeManager);
            //asercja logiki przy za³o¿eniu, ¿e rozszerzenie jest obs³ugiwane
            //...
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