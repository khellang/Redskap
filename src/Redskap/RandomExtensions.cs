using System;
using System.Collections.Generic;

namespace Redskap
{
    internal static class RandomExtensions
    {
        internal static Gender NextGender(this Random random)
        {
            return (Gender) random.Next(2);
        }

        internal static DateTime NextDate(this Random random, DateTime minValue, DateTime maxValue)
        {
            return minValue.AddDays(random.Next((maxValue - minValue).Days + 1));
        }

        internal static IEnumerable<int> GetIndividualNumbers(this Random random, int year)
        {
            if (year < 1854)
            {
                throw new ArgumentException($"Invalid year: {year}", nameof(year));
            }

            if (year <= 1899)
            {
                // 500–749 omfatter personer født i perioden 1854–1899.
                return random.GetRange(500, 749);
            }

            if (year <= 1939)
            {
                // 900–999 omfatter personer født i perioden 1940–1999.
                return random.GetRange(900, 999);
            }

            if (year <= 1999)
            {
                // 000–499 omfatter personer født i perioden 1900–1999.
                return random.GetRange(0, 499);
            }

            if (year <= 2039)
            {
                // 500–999 omfatter personer født i perioden 2000–2039.
                return random.GetRange(500, 999);
            }

            throw new ArgumentException($"Invalid year: {year}", nameof(year));
        }

        private static IEnumerable<int> GetRange(this Random random, int minValue, int maxValue)
        {
            while (true)
            {
                yield return random.Next(minValue, maxValue + 1);
            }
        }
    }
}
