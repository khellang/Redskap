using System;
using System.Collections.Generic;

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
        private static ReadOnlySpan<byte> K2Weights => new byte[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        private static ReadOnlySpan<byte> K1Weights => new byte[] { 3, 7, 6, 1, 8, 9, 4, 5, 2 };

        private const int Length = 11;

        /// <summary>
        /// Determines whether the specified <paramref name="value"/>
        /// is a valid Norwegian identification number.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(ReadOnlySpan<char> value)
        {
            return TryParse(value, out _);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a valid
        /// Norwegian identification number of the specified <paramref name="kind"/>.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="kind">The kind to validate against.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(ReadOnlySpan<char> value, Kind kind)
        {
            return TryParse(value, out var result) && result.NumberKind == kind;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="value"/> is a valid
        /// Norwegian identification number of one of the specified <paramref name="kinds"/>.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="kinds">The kinds to validate against.</param>
        /// <returns><see langword="true"/> if the specified value
        /// is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(ReadOnlySpan<char> value, IEnumerable<Kind> kinds)
        {
            if (TryParse(value, out var result))
            {
                foreach (var kind in kinds)
                {
                    if (result.NumberKind == kind)
                    {
                        return true;
                    }
                }
            }

            return false;
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
        public static bool TryParseGender(ReadOnlySpan<char> value, out Gender gender)
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
        public static bool TryParseDateOfBirth(ReadOnlySpan<char> value, out DateTime dateOfBirth)
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
        public static bool TryParse(ReadOnlySpan<char> value, out IdentificationNumber result)
        {
            return TryParse(value, out result, out _);
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
        public static bool TryParse(ReadOnlySpan<char> value, out IdentificationNumber result, out ParseError error)
        {
            result = default;
            error = default;

            if (value.Length != Length)
            {
                error = ParseError.InvalidLength;
                return false;
            }

            Span<byte> digits = stackalloc byte[Length];

            for (var i = 0; i < Length; i++)
            {
                // Convert ASCII characters to numeric values
                digits[i] = (byte) (value[i] - '0');

                if (digits[i] > 9)
                {
                    error = ParseError.InvalidCharacter;
                    return false;
                }
            }

            var k2 = Checksum.Mod11(digits, K2Weights);
            if (k2 == 10 || k2 != digits[10])
            {
                error = ParseError.InvalidChecksum;
                return false;
            }

            var k1 = Checksum.Mod11(digits, K1Weights);
            if (k1 == 10 || k1 != digits[9])
            {
                error = ParseError.InvalidChecksum;
                return false;
            }

            var individual = digits[8] + (digits[7] * 10) + (digits[6] * 100);
            var year = digits[5] + (digits[4] * 10);

            var fullYear = GetFullYear(year, individual);
            if (!fullYear.HasValue)
            {
                error = ParseError.InvalidYear;
                return false;
            }

            var month = digits[3] + (digits[2] * 10);
            var day = digits[1] + (digits[0] * 10);

            var dateOfBirth = GetDateOfBirth(fullYear.Value, month, day, out var kind, out error);
            if (!dateOfBirth.HasValue)
            {
                return false;
            }

            var checkDigits = (k1 * 10) + k2;
            result = new IdentificationNumber(dateOfBirth.Value, individual, checkDigits, kind);
            return true;
        }

        private static int? GetFullYear(int year, int individual)
        {
            // 000–499 omfatter personer født i perioden 1900–1999.
            if (individual <= 499)
            {
                return 1900 + year;
            }

            // 500–749 omfatter personer født i perioden 1854–1899.
            if (individual <= 749 && year >= 54)
            {
                return 1800 + year;
            }

            // 500–999 omfatter personer født i perioden 2000–2039.
            if (year < 40)
            {
                return 2000 + year;
            }

            // 900–999 omfatter personer født i perioden 1940–1999.
            if (individual >= 900)
            {
                return 1900 + year;
            }

            return null;
        }

        private static DateTime? GetDateOfBirth(int year, int month, int day, out Kind kind, out ParseError error)
        {
            if (day > 40)
            {
                kind = Kind.DNumber;
                return GetDate(year, month, day - 40, out error);
            }

            if (month > 40)
            {
                kind = Kind.HNumber;
                return GetDate(year, month - 40, day, out error);
            }

            kind = Kind.FNumber;
            return GetDate(year, month, day, out error);

            static DateTime? GetDate(int year, int month, int day, out ParseError error)
            {
                // There's no point in validating year, as any non-null
                // result from GetFullYear should be a valid year.

                if (month < 1 || month > 12)
                {
                    error = ParseError.InvalidMonth;
                    return null;
                }

                if (day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    error = ParseError.InvalidDayOfMonth;
                    return null;
                }

                error = default;
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
            }
        }

        /// <summary>
        /// An enum representing an error during parsing of an identification number.
        /// </summary>
        public enum ParseError
        {
            /// <summary>
            /// The identification number has an invalid
            /// length, which should be 11 characters.
            /// </summary>
            InvalidLength,

            /// <summary>
            /// The parser encountered a non-digit character.
            /// </summary>
            InvalidCharacter,

            /// <summary>
            /// One of the two checksum digits did not match the rest of the number.
            /// </summary>
            InvalidChecksum,

            /// <summary>
            /// The year did not match the individual number.
            /// </summary>
            InvalidYear,

            /// <summary>
            /// An invalid month was specified.
            /// </summary>
            InvalidMonth,

            /// <summary>
            /// An invalid day of month was specified.
            /// </summary>
            InvalidDayOfMonth
        }
    }
}
