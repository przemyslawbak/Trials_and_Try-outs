using NUnit.Framework;
using XamNUnit;

namespace Tests
{
    [TestFixture]
    public class XamNUnitTests
    {
        [Test]
        public void Should_Reverse_Word_ReturnsTrue()
        {
            // Arrange (przygotuj)
            string word = "ABC";
            string reversedWord = "CBA";
            ReverseService reverseService = new ReverseService();
            // Act (wykonaj)
            string result = reverseService.ReverseWord(word);
            // Assert (sprawdü)
            Assert.AreEqual(result, reversedWord);
        }
    }
}