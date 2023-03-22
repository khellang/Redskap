using System;
using static Redskap.ParsingHelpers;

namespace Redskap;

public readonly partial struct BankAccountNumber
{
    private static ReadOnlySpan<byte> K1Weights => new byte[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
    
    private const int Length = 11;

    /// <summary>
    /// Determines whether the specified <paramref name="value"/>
    /// is a valid Norwegian bank account number.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns><see langword="true"/> if the specified value
    /// is valid; otherwise, <see langword="false"/>.</returns>
    public static bool IsValid(ReadOnlySpan<char> value)
    {
        return TryParse(value, out _);
    }
    
    /// <summary>
    /// Attempts to parse the specified <paramref name="value"/> into an
    /// <see cref="BankAccountNumber"/> instance. The return value
    /// indicates whether the parsing succeeded. Parsing will fail if
    /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
    /// length, doesn't match the checksum digit or has an otherwise invalid format.
    /// </summary>
    /// <param name="value">A string containing the number to parse.</param>
    /// <param name="result">The resulting <see cref="BankAccountNumber"/> if parsing succeeded.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was
    /// parsed successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> value, out BankAccountNumber result)
    {
        return TryParse(value, out result, out _);
    }

    /// <summary>
    /// Attempts to parse the specified <paramref name="value"/> into an
    /// <see cref="BankAccountNumber"/> instance. The return value
    /// indicates whether the parsing succeeded. Parsing will fail if
    /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
    /// length, doesn't match the checksum digit or has an otherwise invalid format.
    /// </summary>
    /// <param name="value">A string containing the number to parse.</param>
    /// <param name="result">The resulting <see cref="BankAccountNumber"/> if parsing succeeded.</param>
    /// <param name="error">The resulting <see cref="ParseError"/> if parsing failed.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> was
    /// parsed successfully; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryParse(ReadOnlySpan<char> value, out BankAccountNumber result, out ParseError error)
    {
        result = default;
        error = default;
        
        value = value.Trim();

        var length = CountNonSeparatorCharacters(value);
        if (length != Length)
        {
            error = ParseError.InvalidLength;
            return false;
        }

        // We stackalloc after verifying the length to ensure there's enough stack space.
        Span<char> normalized = stackalloc char[Length];

        TrimSeparators(value, normalized);
        
        var k1 = Checksum.Mod11(normalized, K1Weights);
        if (k1 == 10)
        {
            error = ParseError.InvalidChecksum;
            return false;
        }
        
        if (!TryGetDigit(normalized[10], out var k1Digit))
        {
            error = ParseError.InvalidCharacter;
            return false;
        }
        
        if (k1Digit != k1)
        {
            error = ParseError.InvalidChecksum;
            return false;
        }
        
        if (!TryReadFourDecimalDigits(normalized, 6, out var customer))
        {
            error = ParseError.InvalidCharacter;
            return false;
        }
        
        if (!TryReadTwoDecimalDigits(normalized, 4, out var accountGroup))
        {
            error = ParseError.InvalidCharacter;
            return false;
        }
        
        if (!TryReadFourDecimalDigits(normalized, 0, out var register))
        {
            error = ParseError.InvalidCharacter;
            return false;
        }

        result = new BankAccountNumber(register, accountGroup, customer, k1);
        return true;
    }

    private static void TrimSeparators(ReadOnlySpan<char> value, Span<char> normalized)
    {
        var targetIndex = 0;
        
        while (true)
        {
            var target = normalized.Slice(targetIndex);
            
            var index = value.IndexOfAny(' ', '.');
            if (index == -1)
            {
                value.CopyTo(target);
                break;
            }

            value.Slice(0, index).CopyTo(target);
            value = value.Slice(index + 1);
            targetIndex += index;
        }
    }

    private static int CountNonSeparatorCharacters(ReadOnlySpan<char> value)
    {
        var count = 0;
        
        foreach (var @char in value)
        {
            if (@char is ' ' or '.')
            {
                continue;
            }

            count++;
        }

        return count;
    }

    /// <summary>
    /// An enum representing an error during parsing of a bank account number.
    /// </summary>
    public enum ParseError
    {
        /// <summary>
        /// The bank account number has an invalid
        /// length, which should be 11 characters.
        /// </summary>
        InvalidLength,

        /// <summary>
        /// The checksum digit did not match the rest of the number.
        /// </summary>
        InvalidChecksum,

        /// <summary>
        /// The bank account number contains an invalid character.
        /// </summary>
        InvalidCharacter
    }
}
