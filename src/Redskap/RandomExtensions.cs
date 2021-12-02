using System;
using System.Collections;
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

        internal static RandomRangeEnumerator GetIndividualNumbers(this Random random, Gender gender, int year)
        {
            if (year < 1854)
            {
                throw new ArgumentException($"Invalid year: {year}", nameof(year));
            }

            if (year <= 1899)
            {
                // 500–749 omfatter personer født i perioden 1854–1899.
                return random.GetRange(gender, 500, 749);
            }

            if (year <= 1999)
            {
                if (year >= 1940)
                {
                    // 900–999 omfatter personer født i perioden 1940–1999.
                    return random.GetRange(gender, 900, 999);
                }

                // 000–499 omfatter personer født i perioden 1900–1999.
                return random.GetRange(gender, 0, 499);
            }

            if (year <= 2039)
            {
                // 500–999 omfatter personer født i perioden 2000–2039.
                return random.GetRange(gender, 500, 999);
            }

            throw new ArgumentException($"Invalid year: {year}", nameof(year));
        }

        private static RandomRangeEnumerator GetRange(this Random random, Gender gender, int minValue, int maxValue)
        {
            var count = maxValue - minValue + 1;

            var seed = random.Next(0, count);

            // Adjust to an odd or even seed based on the specified gender:
            // - Female is even
            // - Male is odd
            if (gender == Gender.Male && seed % 2 == 0)
            {
                seed = ++seed % count;
            }

            return new RandomRangeEnumerator(minValue, seed, count);
        }

        public struct RandomRangeEnumerator : IEnumerable<int>, IEnumerator<int>
        {
            public RandomRangeEnumerator(int minValue, int seed, int count)
            {
                MinValue = minValue;
                Seed = seed;
                Count = count;
                Index = 0;
            }

            private int MinValue { get; }

            private int Seed { get; }

            private int Count { get; }

            private int Index { get; set; }

            public int Current => MinValue + ((Seed + Index) % Count);

            object IEnumerator.Current => Current;

            public IEnumerator<int> GetEnumerator() => this;

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            public bool MoveNext()
            {
                var hasNext = Index < Count;
                Index += 2;
                return hasNext;
            }

            public void Reset()
            {
                Index = 0;
            }

            public void Dispose()
            {
                // Not needed.
            }
        }
    }
}
