#if NETSTANDARD
using System;

// These methods are convenience methods for netstandard2.0,
// which supports .NET Framework and thus doesn't contain
// ReadOnlySpan<T>. This means there's no implicit
// conversion from string to ReadOnlySpan<char>.

namespace Redskap
{
    public readonly partial struct BankAccountNumber
    {
        /// <summary>
        /// Determines whether the specified <paramref name="value"/>
        /// is a valid Norwegian bank account number.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string? value)
        {
            return value is not null && IsValid(value.AsSpan());
        }

        /// <summary>
        /// Attempts to parse the specified <paramref name="value"/> into an
        /// <see cref="BankAccountNumber"/> instance. The return value
        /// indicates whether the parsing succeeded. Parsing will fail if
        /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
        /// length, doesn't match checksum digit or has an otherwise invalid format.
        /// </summary>
        /// <param name="value">A string containing the number to parse.</param>
        /// <param name="result">The resulting <see cref="BankAccountNumber"/> if parsing succeeded.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? value, out BankAccountNumber result)
        {
            if (value is null)
            {
                result = default;
                return false;
            }

            return TryParse(value.AsSpan(), out result);
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
        public static bool TryParse(string? value, out BankAccountNumber result, out ParseError error)
        {
            if (value is null)
            {
                error = ParseError.InvalidLength;
                result = default;
                return false;
            }

            return TryParse(value.AsSpan(), out result, out error);
        }
    }
}
#endif
