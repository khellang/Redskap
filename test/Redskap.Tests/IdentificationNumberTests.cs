using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Xunit;
using static Redskap.IdentificationNumber;

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
                Assert.True(TryParse(testData.Value, out var result));
                Assert.Equal(testData.DateOfBirth, result.DateOfBirth);
                Assert.Equal(testData.Kind, result.NumberKind);
                Assert.Equal(testData.Gender, result.Gender);
                Assert.Equal(testData.Value, result.ToString());
            }
        }

        [Theory]
        [InlineData("0123456789", ParseError.InvalidLength)] // For kort
        [InlineData("012345678910", ParseError.InvalidLength)] // For langt
        [InlineData("abcdefghijk", ParseError.InvalidCharacter)] // Ugyldig tegn
        [InlineData("01015780000", ParseError.InvalidChecksum)] // Ugyldig individnummer
        [InlineData("01130423880", ParseError.InvalidMonth)] // Ugyldig måned (maks)
        [InlineData("01000434538", ParseError.InvalidMonth)] // Ugyldig måned (min)
        [InlineData("00120467824", ParseError.InvalidDayOfMonth)] // Ugyldig dag (maks)
        [InlineData("32120432426", ParseError.InvalidDayOfMonth)] // Ugyldig dag (min)
        [InlineData("01010101010", ParseError.InvalidChecksum)] // Feil kontrollsiffer
        public void TryParse_Invalid_Returns_False(string identityNumber, ParseError expectedError)
        {
            Assert.False(TryParse(identityNumber, out _, out var error));
            Assert.Equal(expectedError, error);
        }

        [Fact]
        public void Full_Roundtrip()
        {
            var gen = new Generator(new Random(123));

            for (var i = 0; i < 10_000; i++)
            {
                var generated = gen.Generate(Kind.FNumber);

                Assert.True(TryParse(generated.ToString(), out var parsed));

                Assert.Equal(generated.IndividualNumber, parsed.IndividualNumber);
                Assert.Equal(generated.DateOfBirth, parsed.DateOfBirth);
                Assert.Equal(generated.NumberKind, parsed.NumberKind);
                Assert.Equal(generated.Gender, parsed.Gender);

                Assert.Equal(generated.ToString(), parsed.ToString());
            }
        }

        public class TestData
        {
            public TestData(string value, Gender gender, DateTime dateOfBirth, Kind kind)
            {
                Value = value;
                Gender = gender;
                DateOfBirth = dateOfBirth;
                Kind = kind;
            }

            public string Value { get; }

            public Gender Gender { get; }

            public DateTime DateOfBirth { get; }

            public Kind Kind { get; }

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

            private static Kind GetKind(string value)
            {
                return value switch
                {
                    "F" => Kind.FNumber,
                    "D" => Kind.DNumber,
                    "H" => Kind.HNumber,
                    _ => throw new ArgumentException($"Invalid kind: {value}", nameof(value)),
                };
            }
        }
    }
}
