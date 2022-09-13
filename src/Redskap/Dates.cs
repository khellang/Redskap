using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Redskap
{
    /// <summary>
    /// Utility for working with dates and Norwegian holidays.
    /// </summary>
    public static class Dates
    {
        private static readonly ConcurrentDictionary<int, ISet<PublicHoliday>> Holidays = new();

        /// <summary>
        /// Checks whether the specified <paramref name="dateTime"/> is in a weekend, i.e. Saturday or Sunday.
        /// </summary>
        /// <param name="dateTime">The date/time instance to check.</param>
        /// <returns><see langword="true"/> if the date is in a weekend; otherwise, <see langword="false"/>.</returns>
        public static bool IsWeekend(this DateTime dateTime) => dateTime.DayOfWeek.IsWeekend();

        /// <summary>
        /// Checks whether the specified <paramref name="dayOfWeek"/> is in a weekend, i.e. Saturday or Sunday.
        /// </summary>
        /// <param name="dayOfWeek">The <see cref="DayOfWeek"/> instance to check.</param>
        /// <returns><see langword="true"/> if the day is in a weekend; otherwise, <see langword="false"/>.</returns>
        public static bool IsWeekend(this DayOfWeek dayOfWeek) => dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        /// <summary>
        /// Checks whether the specified <paramref name="dateTime"/> is a Norwegian public holiday.
        /// </summary>
        /// <param name="dateTime">The date/time instance to check.</param>
        /// <returns><see langword="true"/> if the date is a Norwegian public holiday; otherwise, <see langword="false"/>.</returns>
        public static bool IsHoliday(this DateTime dateTime) =>
            GetHolidaysForYear(dateTime.Year)
                .Contains(new PublicHoliday(dateTime));

        /// <summary>
        /// Checks whether the specified <paramref name="dateTime"/> is a working
        /// day, i.e. not weekend and not a Norwegian public holiday.
        /// </summary>
        /// <param name="dateTime">The date/time instance to check.</param>
        /// <returns><see langword="true"/> if the date is a working day; otherwise, <see langword="false"/>.</returns>
        public static bool IsWorkingDay(this DateTime dateTime) =>
            !IsWeekend(dateTime) && !IsHoliday(dateTime);

        /// <summary>
        /// Adds the specified amount of <paramref name="workingDays"/> to the specified <paramref name="dateTime"/>.
        /// </summary>
        /// <param name="dateTime">The date/time instance to add to.</param>
        /// <param name="workingDays">The number of working days to add.</param>
        /// <returns>A new <see cref="DateTime"/> with the specified amount of working days added.</returns>
        public static DateTime AddWorkingDays(this DateTime dateTime, int workingDays)
        {
            if (workingDays == 0)
            {
                return dateTime;
            }

            var result = dateTime;
            var sign = Math.Sign(workingDays);

            for (var i = 0; i < Math.Abs(workingDays); i++)
            {
                do { result = result.AddDays(sign); }
                while (!IsWorkingDay(result));
            }

            return result;
        }

        /// <summary>
        /// Counts the number of working days between <paramref name="startDate"/> and <paramref name="endDate"/>.
        /// </summary>
        /// <param name="startDate">The date to start counting working days from.</param>
        /// <param name="endDate">The date to end counting working days on.</param>
        /// <returns>The number of working days between the two specified dates.</returns>
        public static long CountWorkingDaysBetween(DateTime startDate, DateTime endDate)
        {
            if (endDate <= startDate)
            {
                return 0;
            }

            var totalDays = (long) (endDate - startDate).TotalDays;
            var workDays = 0L;

            for (var i = 1; i <= totalDays; i++)
            {
                if (IsWorkingDay(startDate.AddDays(i)))
                {
                    workDays++;
                }
            }

            return workDays;
        }

        /// <summary>
        /// Gets all Norwegian public holidays for the specified <paramref name="year"/>.
        /// </summary>
        /// <param name="year">The year to get all Norwegian public holidays for.</param>
        /// <returns>All Norwegian public holidays for the specified year.</returns>
        public static IEnumerable<PublicHoliday> GetHolidays(int year) => GetHolidaysForYear(year);

        private static ISet<PublicHoliday> GetHolidaysForYear(int year)
        {
            // ReSharper disable once ConvertClosureToMethodGroup
            return Holidays.GetOrAdd(year, y => CreateHolidaySet(y));

            static ISet<PublicHoliday> CreateHolidaySet(int year)
            {
                var easterSunday = CalculateEasterSunday(year);

                return new HashSet<PublicHoliday>(DateOnlyEqualityComparer.Instance)
                {
                    new(year, 1, 1, "Første nyttårsdag", "New Year's Day"),
                    new(easterSunday.AddDays(-3), "Skjærtorsdag", "Maundy Thursday"),
                    new(easterSunday.AddDays(-2), "Langfredag", "Good Friday"),
                    new(easterSunday, "Første påskedag", "Easter Sunday"),
                    new(easterSunday.AddDays(1), "Andre påskedag", "Easter Monday"),
                    new(year, 5, 1, "Første mai", "Labour Day"),
                    new(year, 5, 17, "Syttende mai", "Constitution Day"),
                    new(easterSunday.AddDays(39), "Kristi himmelfartsdag", "Ascension Day"),
                    new(easterSunday.AddDays(49), "Første pinsedag", "Pentecost"),
                    new(easterSunday.AddDays(50), "Andre pinsedag", "Whit Monday"),
                    new(year, 12, 25, "Første juledag", "Christmas Day"),
                    new(year, 12, 26, "Andre juledag", "Saint Stephen's Day"),
                };
            }
        }

        /// <summary>
        /// Based on the Anonymous Gregorian algorithm, also known as the
        /// Meeus/Jones/Butchers algorithm from https://en.wikipedia.org/wiki/Date_of_Easter.
        /// </summary>
        private static DateTime CalculateEasterSunday(int year)
        {
            var a = year % 19;
            var b = year / 100;
            var c = year % 100;
            var d = b / 4;
            var e = b % 4;
            var f = (b + 8) / 25;
            var g = (b - f + 1) / 3;
            var h = ((19 * a) + b - d - g + 15) % 30;
            var i = c / 4;
            var k = c % 4;
            var l = (32 + (2 * e) + (2 * i) - h - k) % 7;
            var m = (a + (11 * h) + (22 * l)) / 451;
            var n = (h + l - (7 * m) + 114) / 31;
            var p = (h + l - (7 * m) + 114) % 31;
            return new DateTime(year, n, p + 1);
        }

        /// <summary>
        /// Equality comparer used as an optimization for checking only day
        /// and month of a date, since we already know the year is equal.
        /// </summary>
        private class DateOnlyEqualityComparer : IEqualityComparer<PublicHoliday>
        {
            public static readonly DateOnlyEqualityComparer Instance = new();

            private DateOnlyEqualityComparer()
            {
            }

            public bool Equals(PublicHoliday x, PublicHoliday y)
            {
                return x.Date.Day.Equals(y.Date.Day) && x.Date.Month.Equals(y.Date.Month);
            }

            public int GetHashCode(PublicHoliday obj)
            {
                return HashCode.Combine(obj.Date.Day, obj.Date.Month);
            }
        }
    }
}
