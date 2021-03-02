using System;

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
        /// <summary>
        /// A class for generating random valid <see cref="IdentificationNumber"/> instances.
        /// </summary>
        public class Generator
        {
            private static readonly DateTime MaxValue = new(2039, 12, 31);

            private static readonly DateTime MinValue = new(1854, 1, 1);

            /// <summary>
            /// Creates a new <see cref="Generator"/> instance using
            /// the specified <paramref name="random"/> number generator.
            /// </summary>
            /// <param name="random">The pseudo-random number generator to use.</param>
            public Generator(Random random) => Random = random;

            private Random Random { get; }

            /// <summary>
            /// Generates a valid <see cref="IdentificationNumber"/> of the specified <paramref name="kind"/>.
            /// </summary>
            /// <param name="kind">The kind of identification number to generate.</param>
            /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
            public IdentificationNumber Generate(Kind kind) =>
                Generate(kind, Random.NextGender());

            /// <summary>
            /// Generates a valid <see cref="IdentificationNumber"/> with the
            /// specified <paramref name="gender"/> and <paramref name="kind"/>.
            /// </summary>
            /// <param name="kind">The kind of identification number to generate.</param>
            /// <param name="gender">The gender to generate an identification number for.</param>
            /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
            public IdentificationNumber Generate(Kind kind, Gender gender) =>
                Generate(kind, gender, MinValue, MaxValue);

            /// <summary>
            /// Generates a valid <see cref="IdentificationNumber"/> with the
            /// specified <paramref name="gender"/>, a date of birth between
            /// <paramref name="minValue"/> and <paramref name="maxValue"/>,
            /// and the specified <paramref name="kind"/>.
            /// </summary>
            /// <param name="kind">The kind of identification number to generate.</param>
            /// <param name="gender">The gender to generate an identification number for.</param>
            /// <param name="minValue">The inclusive lower bound for the date of birth.</param>
            /// <param name="maxValue">The inclusive upper bound for the date of birth.</param>
            /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
            public IdentificationNumber Generate(Kind kind, Gender gender, DateTime minValue, DateTime maxValue) =>
                Generate(kind, gender, Random.NextDate(minValue, maxValue));

            /// <summary>
            /// Generates a valid <see cref="IdentificationNumber"/> with the
            /// specified <paramref name="gender"/>, <paramref name="dateOfBirth"/> and <paramref name="kind"/>.
            /// </summary>
            /// <param name="kind">The kind of identification number to generate.</param>
            /// <param name="gender">The gender to generate an identification number for.</param>
            /// <param name="dateOfBirth">The date of birth to generate an identificatin number for.</param>
            /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
            public IdentificationNumber Generate(Kind kind, Gender gender, DateTime dateOfBirth)
            {
                Span<byte> buffer = stackalloc byte[Length];

                var (day, month, year) = AdjustByKind(dateOfBirth, kind);

                FormattingHelpers.WriteTwoDecimalDigits((uint) day, buffer, 0);
                FormattingHelpers.WriteTwoDecimalDigits((uint) month, buffer, 2);
                FormattingHelpers.WriteTwoDecimalDigits((uint) year, buffer, 4);

                foreach (var individualNumber in Random.GetIndividualNumbers(dateOfBirth.Year))
                {
                    if (GetGender(individualNumber) != gender)
                    {
                        continue;
                    }

                    FormattingHelpers.WriteThreeDecimalDigits((uint) individualNumber, buffer, 6);

                    if (HasValidCheckDigits(buffer, out var checkDigits))
                    {
                        return new IdentificationNumber(dateOfBirth, individualNumber, checkDigits, kind);
                    }
                }

                // This should never happen
                throw new InvalidOperationException("Failed to generate identification number based on the specified constraints.");
            }

            private static bool HasValidCheckDigits(Span<byte> digits, out int checkDigits)
            {
                var k1 = Checksum.Mod11(digits, K1Weights);
                if (k1 == 10)
                {
                    checkDigits = default;
                    return false;
                }

                digits[9] = k1; // Ugh, this feels yucky :(

                var k2 = Checksum.Mod11(digits, K2Weights);
                if (k2 == 10)
                {
                    checkDigits = default;
                    return false;
                }

                checkDigits = (k1 * 10) + k2;
                return true;
            }

            private static (int day, int month, int year) AdjustByKind(DateTime date, Kind kind)
            {
                return kind switch
                {
                    Kind.FNumber => (date.Day, date.Month, date.Year),
                    Kind.DNumber => (date.Day + 40, date.Month, date.Year),
                    Kind.HNumber => (date.Day, date.Month + 40, date.Year),
                    _ => throw new ArgumentException($"Invalid kind: {kind}", nameof(kind)),
                };
            }
        }
    }
}