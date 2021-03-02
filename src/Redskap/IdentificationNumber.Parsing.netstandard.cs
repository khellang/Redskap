#if NETSTANDARD
using System;
using System.Collections.Generic;

// These methods are convenience methods for netstandard2.0,
// which supports .NET Framework and thus doesn't contain
// ReadOnlySpan<T>. This means there's no implicit
// conversion from string to ReadOnlySpan<char>.

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
        /// <summary>
        /// Determines whether the specified <paramref name="value"/>
        /// is a valid Norwegian identification number.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string? value)
        {
            return value is not null && IsValid(value.AsSpan());
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a valid
        /// Norwegian identification number of the specified <paramref name="kind"/>.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="kind">The kind to validate against.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string? value, Kind kind)
        {
            return value is not null && IsValid(value.AsSpan(), kind);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a valid
        /// Norwegian identification number of one of the specified <paramref name="kinds"/>.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="kinds">The kinds to validate against.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string? value, IEnumerable<Kind> kinds)
        {
            return value is not null && IsValid(value.AsSpan(), kinds);
        }

        /// <summary>
        /// Attempts to parse the <paramref name="gender"/> from a
        /// Norwegian identification number. The return value
        /// indicates whether the parsing succeeded. Parsing will fail if
        /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
        /// length, doesn't match checksum digits or has an otherwise invalid format.
        /// </summary>
        /// <param name="value">A string containing the number to parse.</param>
        /// <param name="gender">The resulting <see cref="Gender"/> if parsing succeeded.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParseGender(string? value, out Gender gender)
        {
            if (TryParse(value, out var result))
            {
                gender = result.Gender;
                return true;
            }

            gender = default;
            return false;
        }

        /// <summary>
        /// Attempts to parse the <paramref name="dateOfBirth"/> from a
        /// Norwegian identification number. The return value
        /// indicates whether the parsing succeeded. Parsing will fail if
        /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
        /// length, doesn't match checksum digits or has an otherwise invalid format.
        /// </summary>
        /// <param name="value">A string containing the number to parse.</param>
        /// <param name="dateOfBirth">The resulting <see cref="DateTime"/> if parsing succeeded.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParseDateOfBirth(string? value, out DateTime dateOfBirth)
        {
            if (TryParse(value, out var result))
            {
                dateOfBirth = result.DateOfBirth;
                return true;
            }

            dateOfBirth = default;
            return false;
        }

        /// <summary>
        /// Attempts to parse the specified <paramref name="value"/> into an
        /// <see cref="IdentificationNumber"/> instance. The return value
        /// indicates whether the parsing succeeded. Parsing will fail if
        /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
        /// length, doesn't match checksum digits or has an otherwise invalid format.
        /// </summary>
        /// <param name="value">A string containing the number to parse.</param>
        /// <param name="result">The resulting <see cref="IdentificationNumber"/> if parsing succeeded.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? value, out IdentificationNumber result)
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
        /// <see cref="IdentificationNumber"/> instance. The return value
        /// indicates whether the parsing succeeded. Parsing will fail if
        /// <paramref name="value"/> is <see langword="null"/>, empty, has an invalid
        /// length, doesn't match checksum digits or has an otherwise invalid format.
        /// </summary>
        /// <param name="value">A string containing the number to parse.</param>
        /// <param name="result">The resulting <see cref="IdentificationNumber"/> if parsing succeeded.</param>
        /// <param name="error">The resulting <see cref="ParseError"/> if parsing failed.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? value, out IdentificationNumber result, out ParseError error)
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
