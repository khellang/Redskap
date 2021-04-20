using System;
using Xunit;

namespace Redskap.Tests
{
    public class DatesTests
    {
        [Fact]
        public void IsHoliday_Returns_True()
        {
            AssertHoliday(2021, 01, 01);
            AssertHoliday(2021, 04, 01);
            AssertHoliday(2021, 04, 02);
            AssertHoliday(2021, 04, 04);
            AssertHoliday(2021, 04, 05);
            AssertHoliday(2021, 05, 01);
            AssertHoliday(2021, 05, 13);
            AssertHoliday(2021, 05, 17);
            AssertHoliday(2021, 05, 23);
            AssertHoliday(2021, 05, 24);
            AssertHoliday(2021, 12, 25);
            AssertHoliday(2021, 12, 26);
        }

        [Fact]
        public void AddWorkingDays_Before_Weekend()
        {
            var result = new DateTime(2021, 04, 23).AddWorkingDays(2);
            Assert.Equal(new DateTime(2021, 04, 27), result);
        }

        [Fact]
        public void AddWorkingDays_Before_Easter()
        {
            var result = new DateTime(2021, 03, 31).AddWorkingDays(2);
            Assert.Equal(new DateTime(2021, 04, 07), result);
        }

        [Fact]
        public void CountWorkingDays_Over_Weekend()
        {
            var startDate = new DateTime(2021, 04, 23);
            var endDate = new DateTime(2021, 04, 27);
            var result = Dates.CountWorkingDaysBetween(startDate, endDate);
            Assert.Equal(2, result);
        }

        [Fact]
        public void CountWorkingDays_Over_Easter()
        {
            var startDate = new DateTime(2021, 03, 31);
            var endDate = new DateTime(2021, 04, 07);
            var result = Dates.CountWorkingDaysBetween(startDate, endDate);
            Assert.Equal(2, result);
        }

        private static void AssertHoliday(int year, int month, int day)
        {
            Assert.True(new DateTime(year, month, day).IsHoliday());
        }
    }
}
