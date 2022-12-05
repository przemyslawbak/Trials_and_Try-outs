using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using System;
using Unit_Test_09___KStafford;
using static Unit_Test_09___KStafford.StringCalculator;

namespace TestDemo.UnitTest
{
    //installed NuGet:
    //nUnit
    //Moq
    //link: https://www.youtube.com/watch?v=Oy-Ny1Op6PY

    [TestFixture]
    public class StringCalculator_UnitTests
    {
        //refaktorizing factory method for IStore store parameter in the constructor
        private Mock<IStore> _mockStore;
        private StringCalculator GetCalculator() //fabryka
        {
            _mockStore = new Mock<IStore>();
            var calc = new StringCalculator(_mockStore.Object);
            return calc;
        }
        [Test]
        public void Add_EmptyString_Returns_0()
        {
            StringCalculator calc = GetCalculator();
            int expectedResult = 0;
            int result = calc.Add("");
            Assert.AreEqual(expectedResult, result);
        }
        [TestCase(1, "1")]
        public void Add_SingleNumbers_ReturnsTheNumber(int expected, string input)
        {
            StringCalculator calc = GetCalculator();
            int result = calc.Add(input);
            Assert.AreEqual(expected, result);
        }
        [TestCase("2,3",5)]
        [TestCase("101,20", 121)]
        [TestCase("3,8, 10", 21)]
        [TestCase("1,2,3,4,5,6,7", 28)]
        public void Add_MultipleNumbers_SumOfAllNumbers(string input, int expectedResult)
        {
            StringCalculator calc = GetCalculator();
            int result = calc.Add(input);
            Assert.AreEqual(expectedResult, result);
        }
        [TestCase("a,1")]
        [TestCase("abc,''")]
        [TestCase("-,/")]
        [TestCase("qwerty")]
        [TestCase("1,2,3,\\,/,?,!")]
        //[ExpectedException] <- removed in nUnit 3.x
        public void Add_InvalidString_ThrowsException(string input)
        {
            StringCalculator calc = GetCalculator();
            ActualValueDelegate<object> testDelegate = () => calc.Add(input);
            Assert.That(testDelegate, Throws.TypeOf<ArgumentException>());
        }
        [TestCase("-1", -1)]
        [TestCase("-1, 1", 0)]
        [TestCase("-1, -5, -12", -18)]
        public void MinusNumbers_Scenario_AreSummedCorrectly(string input, int expectedResult)
        {
            StringCalculator calc = GetCalculator();
            var result = calc.Add(input);
            Assert.AreEqual(expectedResult, result);
        }

        //MOCKING

        [Test]
        public void Add_ResultIsAPrimeNumber_ResultsAreSaved()
        {
            StringCalculator calc = GetCalculator(); //passing interface to StringCalculator
            var result = calc.Add("3,4");
            _mockStore.Verify(m => m.Save(It.IsAny<int>()), Times.Once); //Verify if saving total with int type, once
        }
    }
}
