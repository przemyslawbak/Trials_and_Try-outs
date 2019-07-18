using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamNUnit;

namespace UnitTestCore
{
    [TestClass]
    public class ReverseServiceTests
    {
        [TestMethod]
        public void Should_Reverse_Word()
        {
            // Arrange (przygotuj)
            string word = "ABC";
            string reversedWord = "CBA";
            ReverseService reverseService = new ReverseService();
            // Act (wykonaj)
            string result = reverseService.ReverseWord(word);
            // Assert (sprawdź)
            Assert.AreEqual(result, reversedWord);
        }
    }
}
