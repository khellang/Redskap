using System;
using System.Collections.Generic;
using static Redskap.ParsingHelpers;

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
        private static ReadOnlySpan<byte> K2Weights => new byte[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        private static ReadOnlySpan<byte> K1Weights => new byte[] { 3, 7, 6, 1, 8, 9, 4, 5, 2 };

        private const int DhNumberBase = 40;

        private const int MinDayOfMonth = 1;

        private const int MinMonth = 1;

        private const int MaxMonth = 12;

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

            var k2 = Checksum.Mod11(value, K2Weights);
            if (k2 == 10)
            {
                error = ParseError.InvalidChecksum;
                return false;
            }

            if (!TryGetDigit(value[10], out var k2Digit))
            {
                error = ParseError.InvalidCharacter;
                return false;
            }

            if (k2Digit != k2)
            {
                error = ParseError.InvalidChecksum;
                return false;
            }

            var k1 = Checksum.Mod11(value, K1Weights);
            if (k1 == 10)
            {
                error = ParseError.InvalidChecksum;
                return false;
            }

            if (!TryGetDigit(value[9], out var k1Digit))
            {
                error = ParseError.InvalidCharacter;
                return false;
            }

            if (k1Digit != k1)
            {
                error = ParseError.InvalidChecksum;
                return false;
            }

            if (!TryReadThreeDecimalDigits(value, 6, out var individual))
            {
                error = ParseError.InvalidCharacter;
                return false;
            }

            if (!TryReadTwoDecimalDigits(value, 4, out var year))
            {
                error = ParseError.InvalidCharacter;
                return false;
            }

            var century = GetCentury(year, individual);
            if (!century.HasValue)
            {
                error = ParseError.InvalidYear;
                return false;
            }

            // There's no need to validate year, as any non-null
            // result from GetCentury should result in a valid year.
            year += century.Value;

            if (!TryReadTwoDecimalDigits(value, 2, out var month))
            {
                error = ParseError.InvalidCharacter;
                return false;
            }

            var kind = Kind.FNumber;

            if (month > DhNumberBase)
            {
                kind = Kind.HNumber; // H numbers get 40 added to the month.
                month -= DhNumberBase;
            }

            if (month is < MinMonth or > MaxMonth)
            {
                error = ParseError.InvalidMonth;
                return false;
            }

            if (!TryReadTwoDecimalDigits(value, 0, out var day))
            {
                error = ParseError.InvalidCharacter;
                return false;
            }

            if (day > DhNumberBase)
            {
                kind = Kind.DNumber; // D numbers get 40 added to the day.
                day -= DhNumberBase;
            }

            if (day < MinDayOfMonth || day > DateTime.DaysInMonth(year, month))
            {
                error = ParseError.InvalidDayOfMonth;
                return false;
            }

            var checkDigits = (k1 * 10) + k2;
            var dateOfBirth = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
            result = new IdentificationNumber(dateOfBirth, individual, checkDigits, kind);
            return true;
        }

        private static int? GetCentury(int year, int individual)
        {
            // 000–499 omfatter personer født i perioden 1900–1999.
            if (individual <= 499)
            {
                return 1900;
            }

            // 500–749 omfatter personer født i perioden 1854–1899.
            if (individual <= 749 && year >= 54)
            {
                return 1800;
            }

            // 500–999 omfatter personer født i perioden 2000–2039.
            if (year <= 39)
            {
                return 2000;
            }

            // 900–999 omfatter personer født i perioden 1940–1999.
            if (individual >= 900)
            {
                return 1900;
            }

            return null;
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
            InvalidDayOfMonth,

            /// <summary>
            /// The identification number contains an invalid character.
            /// </summary>
            InvalidCharacter
        }
    }
}
