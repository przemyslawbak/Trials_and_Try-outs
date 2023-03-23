namespace Activator
{
    class Program
    {
        static void Main(string[] args)
        {
            var timeZoneOffsetPoland = GetTimeSpanToUtc("Central European Standard Time");
            var rollingHourPoland = 1;
            var month = GetUtcMonthNow();
            var year = GetUtcYearNow();
            var thirdFridayOfMonth = GetThirdFriday(year, month);
            var thirdFridayOfNextMonth = GetThirdFriday(year, month + 1);
            var thirdFridayOfAnotherMonth = GetThirdFriday(year, month + 2);

            if (thirdFridayOfMonth.AddHours(rollingHourPoland) < DateTime.UtcNow + timeZoneOffsetPoland)
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



