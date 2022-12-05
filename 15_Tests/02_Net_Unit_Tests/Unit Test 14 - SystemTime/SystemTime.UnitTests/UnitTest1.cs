using NUnit.Framework;
using System;
using Unit_Test_14___SystemTime;

namespace Tests
{
    [TestFixture]
    public class TimeLoggerTests
    {
        [Test]
        public void SettingSystemTime_Always_ChangesTime()
        {
            SystemTime.Set(new DateTime(2000, 1, 1)); //Ustawienie sztucznej daty
            string output = TimeLogger.CreateMessage("a");
            StringAssert.Contains("01.01.2000", output);
        }
        [TearDown]
        public void AfterEachTest()
        {
            SystemTime.Reset(); //Zresetowanie daty na koñcu ka¿dego testu
        }
    }
}