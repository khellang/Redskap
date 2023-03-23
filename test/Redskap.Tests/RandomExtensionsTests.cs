using System;
using System.Collections.Generic;
using Xunit;

namespace Redskap.Tests;

public class RandomExtensionsTests
{
    [Theory]
    [MemberData(nameof(IndividualNumberData))]
    public static void GetIndividualNumbersTests(Gender gender, int year)
    {
        var random = new Random(123);

        var set = new HashSet<int>();
        var individualNumbers = random.GetIndividualNumbers(gender, year);

        while (individualNumbers.MoveNext())
        {
            var number = individualNumbers.Current;

            Assert.Equal(gender == Gender.Female, number % 2 == 0); // Female should be even
            Assert.True(set.Add(number)); // No duplicates
        }

        var start = individualNumbers.MinValue;

        if (gender == Gender.Male)
        {
            start++; // Adjust to odd numbers for males.
        }

        // The entire range should be present
        for (var i = start; i <= individualNumbers.MaxValue; i += 2)
        {
            Assert.Contains(i, set);
        }
    }

    public static IEnumerable<object[]> IndividualNumberData
    {
        get
        {
            foreach (var year in IdentificationNumber.MinYear..IdentificationNumber.MaxYear)
            {
                yield return new object[] { Gender.Male, year };
            }

            foreach (var year in IdentificationNumber.MinYear..IdentificationNumber.MaxYear)
            {
                yield return new object[] { Gender.Female, year };
            }
        }
    }
}