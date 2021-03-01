using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;

namespace Redskap.Tests
{
    public class IdentificationNumberTests
    {
        [Theory]
        [InlineData("TestData/testdata-f.txt")]
        [InlineData("TestData/testdata-d.txt")]
        [InlineData("TestData/testdata-h.txt")]
        public void TryParse_Valid_Returns_True(string path)
        {
            foreach (var testData in TestData.ReadFrom(path))
            {
                Assert.True(IdentificationNumber.TryParse(testData.Value, out var result));
                Assert.Equal(testData.DateOfBirth, result.DateOfBirth);
                Assert.Equal(testData.Gender, result.Gender);
            }
        }

        [Theory]
        [InlineData("0123456789", "For kort")]
        [InlineData("012345678910", "For langt")]
        [InlineData("abcdefghijk", "Ugyldig tegn")]
        [InlineData("01015780000", "Ugyldig individnummer")]
        [InlineData("01130400000", "Ugyldig måned (maks)")]
        [InlineData("01000400000", "Ugyldig måned (min)")]
        [InlineData("00120467800", "Ugyldig dag (maks)")]
        [InlineData("32120400000", "Ugyldig dag (min)")]
        [InlineData("01010101010", "Feil kontrollsiffer")]
        public void TryParse_Invalid_Returns_False(string identityNumber, string description)
        {
            Assert.False(IdentificationNumber.TryParse(identityNumber, out _), description);
        }

        public class TestData
        {
            public TestData(string value, Gender gender, DateTime dateOfBirth, IdentificationNumberKind kind)
            {
                Value = value;
                Gender = gender;
                DateOfBirth = dateOfBirth;
                Kind = kind;
            }

            public string Value { get; }

            public Gender Gender { get; }

            public DateTime DateOfBirth { get; }

            public IdentificationNumberKind Kind { get; }

            public static IEnumerable<TestData> ReadFrom(string fileName)
            {
                foreach (var line in File.ReadLines(fileName))
                {
                    var parts = line.Split('\t');

                    var value = parts[0];
                    var gender = GetGender(parts[1]);
                    var dateOfBirth = GetDateOfBirth(parts[2]);
                    var kind = GetKind(parts[3]);

                    yield return new TestData(value, gender, dateOfBirth, kind);
                }
            }

            public override string ToString()
            {
                return Value;
            }

            private static Gender GetGender(string value)
            {
                return string.Equals(value, "M", StringComparison.OrdinalIgnoreCase) ? Gender.Male : Gender.Female;
            }

            private static DateTime GetDateOfBirth(string value)
            {
                return DateTime.ParseExact(value, "d.M.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal);
            }

            private static IdentificationNumberKind GetKind(string value)
            {
                return value switch
                {
                    "F" => IdentificationNumberKind.FNumber,
                    "D" => IdentificationNumberKind.DNumber,
                    "H" => IdentificationNumberKind.HNumber,
                    _ => throw new ArgumentException($"Invalid kind: {value}", nameof(value)),
                };
            }
        }
    }
}
