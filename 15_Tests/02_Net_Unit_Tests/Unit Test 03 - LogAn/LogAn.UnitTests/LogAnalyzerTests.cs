using LogAn;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class LogAnalyzerTests
    {
        [Test]
        [Category("Szybkie testy")]
        public void IsValidLogFileName_BadExtension_ReturnsFalse()
        {
            LogAnalyzer analyzer = new LogAnalyzer();
            bool result = analyzer.IsValidLogFileNameee("test.txt");
            Assert.False(result);
        }
        [Test]
        [Category("Szybkie testy")] //s65
        public void IsValidLogFileName_GoodExtensionLowercase_ReturnsTrue()
        {
            LogAnalyzer analyzer = new LogAnalyzer();
            bool result = analyzer
            .IsValidLogFileNameee("filewithgoodextension.slf");
            Assert.True(result);
        }
        [TestCase("filewithgoodextension.SLF")]
        [TestCase("filewithgoodextension.slf")]
        public void IsValidLogFileName_GoodExtensionUppercase_ReturnsTrue(string file)
        {
            LogAnalyzer analyzer = new LogAnalyzer();
            bool result = analyzer.IsValidLogFileNameee(file);
            Assert.True(result);
        }
        [TestCase("filewithgoodextension.foo", false)]
        public void IsValidLogFileName_GoodExtensionUppercase_ReturnsTrue(string file, bool expected)
        {
            LogAnalyzer analyzer = new LogAnalyzer();
            bool result = analyzer.IsValidLogFileNameee(file);
            Assert.AreEqual(expected, result);
        }
        [Test]
        public void IsValidFileName_EmptyFileName_Throws() //s62
        {
            LogAnalyzer la = new LogAnalyzer();
            var ex = Assert.Throws<ArgumentException>(() => la.IsValidLogFileNameee(""));
            StringAssert.Contains("nale¿y podaæ nazwê pliku", ex.Message);
        }
        //coœ tu nie teges
        [Test]
        public void
        IsValidFileName_WhenCalled_ChangesWasLastFileNameValid()
        {
            LogAnalyzer la = new LogAnalyzer();
            la.IsValidLogFileNameee("badname.slf");
            Assert.True(la.WasLastFileNameValid);
        }
        [TestCase("badfile.foo", false)]
        [TestCase("goodfile.slf", true)]
        public void
        IsValidFileName_WhenCalled_ChangesWasLastFileNameValid(string file,
        bool expected)
        {
            LogAnalyzer la = MakeAnalyzer();
            la.IsValidLogFileNameee(file);
            Assert.AreEqual(expected, la.WasLastFileNameValid);
        }
        private LogAnalyzer MakeAnalyzer() //metoda-fabryka
        {
            return new LogAnalyzer(); //tworzy egzemplarz klasy LogAnalyzer
        }
    }
}