using System;
using static Redskap.FormattingHelpers;

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
#if NET6_0_OR_GREATER
        private static readonly Generator Gen = new(Random.Shared);
#else
        private static readonly Generator Gen = new(new Random());
#endif

        /// <summary>
        /// Generates a valid <see cref="IdentificationNumber"/> of the specified <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">The kind of identification number to generate.</param>
        /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
        public static IdentificationNumber Generate(Kind kind) =>
            Gen.Generate(kind);

        /// <summary>
        /// Generates a valid <see cref="IdentificationNumber"/> with the
        /// specified <paramref name="gender"/> and <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">The kind of identification number to generate.</param>
        /// <param name="gender">The gender to generate an identification number for.</param>
        /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
        public static IdentificationNumber Generate(Kind kind, Gender gender) =>
            Gen.Generate(kind, gender);

        /// <summary>
        /// Generates a valid <see cref="IdentificationNumber"/> with a date of birth
        /// between <paramref name="minValue"/> and <paramref name="maxValue"/>,
        /// and the specified <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">The kind of identification number to generate.</param>
        /// <param name="minValue">The inclusive lower bound for the date of birth.</param>
        /// <param name="maxValue">The inclusive upper bound for the date of birth.</param>
        /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
        public static IdentificationNumber Generate(Kind kind, DateTime minValue, DateTime maxValue) =>
            Gen.Generate(kind, minValue, maxValue);

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
        public static IdentificationNumber Generate(Kind kind, Gender gender, DateTime minValue, DateTime maxValue) =>
            Gen.Generate(kind, gender, minValue, maxValue);

        /// <summary>
        /// Generates a valid <see cref="IdentificationNumber"/> with the
        /// specified <paramref name="dateOfBirth"/> and <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">The kind of identification number to generate.</param>
        /// <param name="dateOfBirth">The date of birth to generate an identificatin number for.</param>
        /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
        public static IdentificationNumber Generate(Kind kind, DateTime dateOfBirth) =>
            Gen.Generate(kind, dateOfBirth);

        /// <summary>
        /// Generates a valid <see cref="IdentificationNumber"/> with the
        /// specified <paramref name="gender"/>, <paramref name="dateOfBirth"/> and <paramref name="kind"/>.
        /// </summary>
        /// <param name="kind">The kind of identification number to generate.</param>
        /// <param name="gender">The gender to generate an identification number for.</param>
        /// <param name="dateOfBirth">The date of birth to generate an identificatin number for.</param>
        /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
        public static IdentificationNumber Generate(Kind kind, Gender gender, DateTime dateOfBirth) =>
            Gen.Generate(kind, gender, dateOfBirth);

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
            /// Generates a valid <see cref="IdentificationNumber"/> with a date of birth
            /// between <paramref name="minValue"/> and <paramref name="maxValue"/>,
            /// and the specified <paramref name="kind"/>.
            /// </summary>
            /// <param name="kind">The kind of identification number to generate.</param>
            /// <param name="minValue">The inclusive lower bound for the date of birth.</param>
            /// <param name="maxValue">The inclusive upper bound for the date of birth.</param>
            /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
            public IdentificationNumber Generate(Kind kind, DateTime minValue, DateTime maxValue) =>
                Generate(kind, Random.NextGender(), minValue, maxValue);

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
            /// specified <paramref name="dateOfBirth"/> and <paramref name="kind"/>.
            /// </summary>
            /// <param name="kind">The kind of identification number to generate.</param>
            /// <param name="dateOfBirth">The date of birth to generate an identificatin number for.</param>
            /// <returns>A valid <see cref="IdentificationNumber"/>.</returns>
            public IdentificationNumber Generate(Kind kind, DateTime dateOfBirth) =>
                Generate(kind, Random.NextGender(), dateOfBirth);

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
                Span<char> buffer = stackalloc char[Length];

                WriteDateOfBirth(buffer, dateOfBirth, kind);

                var enumerator = Random.GetIndividualNumbers(dateOfBirth.Year);

                while (enumerator.MoveNext())
                {
                    var individualNumber = enumerator.Current;

                    if (GetGender(individualNumber) != gender)
                    {
                        continue;
                    }

                    WriteThreeDecimalDigits((uint) individualNumber, buffer, 6);

                    if (HasValidCheckDigits(buffer, out var checkDigits))
                    {
                        return new IdentificationNumber(dateOfBirth, individualNumber, checkDigits, kind);
                    }
                }

                // This should never happen
                throw new InvalidOperationException("Failed to generate identification number based on the specified constraints.");
            }

            private static bool HasValidCheckDigits(Span<char> buffer, out int checkDigits)
            {
                var k1 = Checksum.Mod11(buffer, K1Weights);
                if (k1 == 10)
                {
                    checkDigits = default;
                    return false;
                }

                buffer[9] = GetChar(k1); // Ugh, this feels yucky :(

                var k2 = Checksum.Mod11(buffer, K2Weights);
                if (k2 == 10)
                {
                    checkDigits = default;
                    return false;
                }

                checkDigits = (k1 * 10) + k2;
                return true;
            }
        }
    }
}
