using System;

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
        private static ReadOnlySpan<byte> K2Weights => new byte[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        private static ReadOnlySpan<byte> K1Weights => new byte[] { 3, 7, 6, 1, 8, 9, 4, 5, 2 };

        private const int Length = 11;

#if NETSTANDARD
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
        /// <param name="error">The resulting <see cref="Error"/> if parsing failed.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(string? value, out IdentificationNumber result, out Error error)
        {
            if (value is null)
            {
                error = Error.InvalidLength;
                result = default;
                return false;
            }

            return TryParse(value.AsSpan(), out result, out error);
        }
#endif

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
        /// <param name="error">The resulting <see cref="Error"/> if parsing failed.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> was
        /// parsed successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryParse(ReadOnlySpan<char> value, out IdentificationNumber result, out Error error)
        {
            result = default;
            error = default;

            if (value.Length != Length)
            {
                error = Error.InvalidLength;
                return false;
            }

            Span<byte> digits = stackalloc byte[Length];

            for (var i = 0; i < Length; i++)
            {
                // Convert ASCII characters to numeric values
                digits[i] = (byte) (value[i] - '0');

                if (digits[i] > 9)
                {
                    error = Error.InvalidCharacter;
                    return false;
                }
            }

            var k2 = Checksum.Mod11(digits, K2Weights);
            if (k2 == 10 || k2 != digits[10])
            {
                error = Error.InvalidChecksum;
                return false;
            }

            var k1 = Checksum.Mod11(digits, K1Weights);
            if (k1 == 10 || k1 != digits[9])
            {
                error = Error.InvalidChecksum;
                return false;
            }

            var day = (digits[0] * 10) + digits[1];
            var month = (digits[2] * 10) + digits[3];
            var year = (digits[4] * 10) + digits[5];
            var individual = (digits[6] * 100) + (digits[7] * 10) + digits[8];

            var fullYear = GetFullYear(year, individual);
            if (!fullYear.HasValue)
            {
                error = Error.InvalidYear;
                return false;
            }

            var dateOfBirth = GetDateOfBirth(fullYear.Value, month, day, out var kind, out error);
            if (!dateOfBirth.HasValue)
            {
                return false;
            }

            result = new IdentificationNumber(dateOfBirth.Value, individual, kind);
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

        private static DateTime? GetDateOfBirth(int year, int month, int day, out IdentificationNumberKind kind, out Error error)
        {
            if (day > 40)
            {
                kind = IdentificationNumberKind.DNumber;
                return GetDate(year, month, day - 40, out error);
            }

            if (month > 40)
            {
                kind = IdentificationNumberKind.HNumber;
                return GetDate(year, month - 40, day, out error);
            }

            kind = IdentificationNumberKind.FNumber;
            return GetDate(year, month, day, out error);

            static DateTime? GetDate(int year, int month, int day, out Error error)
            {
                // There's no point in validating year, as any non-null
                // result from GetFullYear should be a valid year.

                if (month < 1 || month > 12)
                {
                    error = Error.InvalidMonth;
                    return null;
                }

                if (day < 1 || day > DateTime.DaysInMonth(year, month))
                {
                    error = Error.InvalidDayOfMonth;
                    return null;
                }

                error = default;
                return new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Local);
            }
        }

        /// <summary>
        /// An enum representing an error during parsing of an identification number.
        /// </summary>
        public enum Error
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
