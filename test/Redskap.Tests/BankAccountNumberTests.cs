using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using static Redskap.BankAccountNumber;

namespace Redskap.Tests;

public class BankAccountNumberTests
{
    [Theory]
    [InlineData("TestData/testdata-account.txt")]
    public void TryParse_Valid_Returns_True(string path)
    {
        foreach (var testData in TestData.ReadFrom(path))
        {
            Assert.True(TryParse(testData.Value, out var result));
            Assert.Equal(testData.RegisterNumber, result.RegisterNumber);
            Assert.Equal(testData.AccountGroup, result.AccountGroup);
            Assert.Equal(testData.CustomerNumber, result.CustomerNumber);
            Assert.Equal(testData.CheckDigit, result.CheckDigit);
            // Assert.Equal(testData.Value, result.ToString());
        }
    }

    [Theory]
    [InlineData("0123456789", ParseError.InvalidLength)] // For kort
    [InlineData("012345678910", ParseError.InvalidLength)] // For langt
    [InlineData("abcdefghijk", ParseError.InvalidCharacter)] // Ugyldig tegn
    [InlineData("01010101010", ParseError.InvalidChecksum)] // Feil kontrollsiffer
    public void TryParse_Invalid_Returns_False(string bankAccountNumber, ParseError expectedError)
    {
        Assert.False(TryParse(bankAccountNumber, out _, out var error));
        Assert.Equal(expectedError, error);
    }

    public class TestData
    {
        private TestData(string value, int registerNumber, int accountGroup, int customerNumber, int checkDigit)
        {
            Value = value;
            RegisterNumber = registerNumber;
            AccountGroup = accountGroup;
            CustomerNumber = customerNumber;
            CheckDigit = checkDigit;
        }

        public string Value { get; }

        public int RegisterNumber { get; }

        public int AccountGroup { get; }

        public int CustomerNumber { get; }

        public int CheckDigit { get; }

        public static IEnumerable<TestData> ReadFrom(string fileName)
        {
            foreach (var line in File.ReadLines(fileName))
            {
                var registerNumber = int.Parse(line.AsSpan().Slice(0, 4));
                var accountGroup = int.Parse(line.AsSpan().Slice(4, 2));
                var customerNumber = int.Parse(line.AsSpan().Slice(6, 4));
                var checkDigit = int.Parse(line.AsSpan().Slice(10, 1));

                yield return new TestData(line, registerNumber, accountGroup, customerNumber, checkDigit);
            }
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
