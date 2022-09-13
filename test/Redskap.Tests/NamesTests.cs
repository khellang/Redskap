using System;
using System.Linq;
using Xunit;

namespace Redskap.Tests
{
    public class NamesTests
    {
        public NamesTests()
        {
            Generator = new Names.Generator(new Random(42));
        }

        private Names.Generator Generator { get; }

        [Fact]
        public void GenerateGivenName_Generates_Valid_Female_Name()
        {
            AssertValidName(Generator.GenerateGivenName(Gender.Female));
        }

        [Fact]
        public void GenerateGivenName_Generates_Valid_Male_Name()
        {
            AssertValidName(Generator.GenerateGivenName(Gender.Male));
        }

        [Fact]
        public void GenerateFullName_Generates_Valid_Female_Name()
        {
            AssertValidName(Generator.GenerateFullName(Gender.Female));
        }

        [Fact]
        public void GenerateFullName_Generates_Valid_Male_Name()
        {
            AssertValidName(Generator.GenerateFullName(Gender.Male));
        }

        [Fact]
        public void GenerateFamilyName_Generates_Valid_Name()
        {
            AssertValidName(Generator.GenerateFamilyName());
        }

        [Theory]
        [InlineData(Gender.Male, 10, 30)]
        [InlineData(Gender.Female, 20, 30)]
        public void GenerateGivenName_Generates_Some_Amount_Of_Double_Names(Gender gender, int low, int high)
        {
            var names = Enumerable.Range(0, 100)
                .Select(_ => Generator.GenerateGivenName(gender))
                .Count(x => x.Contains(' '));

            Assert.InRange(names, low, high);
        }

        [Fact]
        public void GenerateGivenName_Generates_Some_Amount_Of_Hyphenated_Names()
        {
            var names = Enumerable.Range(0, 100)
                .Select(_ => Generator.GenerateGivenName(Gender.Male))
                .Count(x => x.Contains('-'));

            Assert.InRange(names, 0, 5);
        }

        [Fact]
        public void GenerateFamilyName_Generates_Some_Amount_Of_Double_Names()
        {
            var names = Enumerable.Range(0, 100)
                .Select(_ => Generator.GenerateFamilyName())
                .Count(x => x.Contains(' '));

            Assert.InRange(names, 0, 15);
        }

        [Fact]
        public void GenerateFamilyName_Generates_Some_Amount_Of_Hyphenated_Names()
        {
            var names = Enumerable.Range(0, 100)
                .Select(_ => Generator.GenerateFamilyName())
                .Count(x => x.Contains('-'));

            Assert.InRange(names, 5, 15);
        }

        private static void AssertValidName(string name)
        {
            Assert.Matches("([A-Z][a-z]+[ -]?)*", name);
        }
    }
}
