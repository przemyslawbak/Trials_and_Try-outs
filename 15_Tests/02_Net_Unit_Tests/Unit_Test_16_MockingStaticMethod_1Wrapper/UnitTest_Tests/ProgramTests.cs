using UnitTest_;
using Xunit;

namespace UnitTest_Tests
{
    public class ProgramTests
    {
        private readonly Program _program;

        public ProgramTests()
        {
            _program = new Program();
        }

        [Fact]
        public void PassingTest()
        {
            Assert.Equal(4, _program.Add(2, 2));
        }

        [Fact]
        public void FailingTest()
        {
            Assert.NotEqual(5, _program.Add(2, 2));
        }
    }
}
