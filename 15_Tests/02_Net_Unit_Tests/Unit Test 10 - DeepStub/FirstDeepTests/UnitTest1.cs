using NUnit.Framework;
using Unit_Test_10___DeepStub;

namespace Tests
{
    [TestFixture]
    public class FirstDeep_UnitTests
    {
        [Test]
        public void AddATest()
        {
            string expected = "ABCAAA";
            //'In the test you use the stub:'
            var firstDeep = new FirstDeep(new SecondDeepStub());
            //'In production code you use the "real" SecondDeep:'
            //'var firstDeep = new FirstDeep(new SecondDeep());'
            string res = firstDeep.AddA("ABC");
            Assert.AreEqual(expected, res);
        }
        public class SecondDeepStub : ISecondDeep //stub implementuje interfejs
        {

            public bool SomethingToDo(string str)
            {
                return true;
            }

        }
    }
}