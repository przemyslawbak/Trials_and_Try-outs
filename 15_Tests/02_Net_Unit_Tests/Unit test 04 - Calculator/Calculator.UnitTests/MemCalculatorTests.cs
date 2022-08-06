using NUnit.Framework;
using Calculator;

namespace Tests
{
    public class MemCalculatorTests //s68
    {
        [Test]
        public void Sum_ByDefault_ReturnsZero()
        {
            MemCalculator calc = MakeCalc();
            int lastSum = calc.Sum();
            Assert.AreEqual(0, lastSum);
        }
        [Test]
        public void Add_WhenCalled_ChangesSum()
        {
            MemCalculator calc = MakeCalc();
            calc.Add(1);
            int sum = calc.Sum();
            Assert.AreEqual(1, sum);
        }
        //metoda fabryka, mo¿na zmieniæ w 1 miejscu jak produkcja siê zmieni
        private static MemCalculator MakeCalc()
        {
            return new MemCalculator();
        }
    }
}