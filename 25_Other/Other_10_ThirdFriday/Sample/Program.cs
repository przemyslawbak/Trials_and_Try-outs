namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> futuresRollingMonthsPoland = new List<int>() { 3, 6, 9, 12 };

            var timeZoneOffsetPoland = GetTimeSpanToUtc("Central European Standard Time");
            var rollingHourPoland = 1;
            var month = GetClosestGreaterMonthNumber(GetUtcMonthNow(), futuresRollingMonthsPoland);
            var nextMonth = GetClosestGreaterMonthNumber(month + 1, futuresRollingMonthsPoland);
            var anotherMonth = GetClosestGreaterMonthNumber(nextMonth + 1, futuresRollingMonthsPoland);

            var year = GetUtcYearNow();

            var thirdFridayOfMonth = GetThirdFriday(year, month);
            var thirdFridayOfNextMonth = GetThirdFriday(year, nextMonth);
            var thirdFridayOfAnotherMonth = GetThirdFriday(year, anotherMonth);

            if (thirdFridayOfMonth.AddHours(rollingHourPoland) > DateTime.UtcNow + timeZoneOffsetPoland)
            {
                Console.WriteLine(thirdFridayOfNextMonth);
            }
            else
            {
                Console.WriteLine(thirdFridayOfAnotherMonth.AddHours(rollingHourPoland));
            }
        }
        public static TimeSpan GetTimeSpanToUtc(string timeZone)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return timeZoneInfo.GetUtcOffset(DateTime.UtcNow);
        }

        private static int GetClosestGreaterMonthNumber(int month, List<int> monthsNosList)
        {
            return monthsNosList.SkipWhile(p => p <= month).FirstOrDefault() != 0
                ? monthsNosList.SkipWhile(p => p <= month).FirstOrDefault()
                : monthsNosList.First();
        }

        private static DateTime GetThirdFriday(int year, int month)
        {
            DateTime day = new DateTime(year, month, 1);

            var counter = 0;

            while (counter != 3)
            {
                if (day.DayOfWeek == DayOfWeek.Friday)
                {
                    counter++;
                }

                if (counter == 3)
                {
                    return day;
                }

                day = day.AddDays(1);
            }

            return day;
        }

        private static int GetUtcYearNow()
        {
            return DateTime.UtcNow.Year;
        }

        private static int GetUtcMonthNow()
        {
            return DateTime.UtcNow.Month;
        }
    }
}



