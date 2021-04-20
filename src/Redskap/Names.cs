using System;

namespace Redskap
{
    /// <summary>
    /// Utility for generating random Norwegian names.
    /// </summary>
    public static class Names
    {
        private static readonly Generator Gen = new(new Random());

        /// <summary>
        /// Generates a random Norwegian family name.
        /// </summary>
        /// <returns>A random Norwegian family name.</returns>
        public static string GenerateFamilyName() => Gen.GenerateFamilyName();

        /// <summary>
        /// Generates a random Norwegian given name.
        /// </summary>
        /// <returns>A random Norwegian given name.</returns>
        public static string GenerateGivenName() => Gen.GenerateGivenName();

        /// <summary>
        /// Generates a Norwegian given name for the specified <paramref name="gender"/>.
        /// </summary>
        /// <param name="gender">The gender to generate a given name for.</param>
        /// <returns>A random Norwegian given name for specified <paramref name="gender"/>.</returns>
        public static string GenerateGivenName(Gender gender) => Gen.GenerateGivenName(gender);

        /// <summary>
        /// Generates a random Norwegian full name.
        /// </summary>
        /// <returns>A random Norwegian full name.</returns>
        public static string GenerateFullName() => Gen.GenerateFullName();

        /// <summary>
        /// Generates a random Norwegian full name for the specified <paramref name="gender"/>.
        /// </summary>
        /// <returns>A random Norwegian full name for specified <paramref name="gender"/>.</returns>
        public static string GenerateFullName(Gender gender) => Gen.GenerateFullName(gender);

        /// <summary>
        /// A class for generating random Norwegian names.
        /// </summary>
        public class Generator
        {
            private const double FemaleMultipleGivenNamesPercent = 22.8;

            private const double MaleMultipleGivenNamesPercent = 19.0;

            private const double HyphenatedGivenNamesPercent = 0.8;

            private const double MultipleFamilyNamesPercent = 11.5;

            private const double HyphenatedFamilyNamesPercent = 8.4;

            /// <summary>
            /// Creates a new <see cref="Generator"/> instance using
            /// the specified <paramref name="random"/> number generator.
            /// </summary>
            /// <param name="random">The pseudo-random number generator to use.</param>
            public Generator(Random random) => Random = random;

            private Random Random { get; }

            /// <summary>
            /// Generates a random Norwegian family name.
            /// </summary>
            /// <returns>A random Norwegian family name.</returns>
            public string GenerateFamilyName() =>
                GetName(FamilyNames.All, MultipleFamilyNamesPercent, HyphenatedFamilyNamesPercent);

            /// <summary>
            /// Generates a random Norwegian given name.
            /// </summary>
            /// <returns>A random Norwegian given name.</returns>
            public string GenerateGivenName() => GenerateGivenName(Random.NextGender());

            /// <summary>
            /// Generates a random Norwegian given name for the specified <paramref name="gender"/>.
            /// </summary>
            /// <param name="gender">The gender to generate a given name for.</param>
            /// <returns>A random Norwegian given name for specified <paramref name="gender"/>.</returns>
            public string GenerateGivenName(Gender gender) => gender switch
            {
                Gender.Female => GetName(FemaleNames.All, FemaleMultipleGivenNamesPercent, HyphenatedGivenNamesPercent),
                Gender.Male => GetName(MaleNames.All, MaleMultipleGivenNamesPercent, HyphenatedGivenNamesPercent),
                _ => throw new ArgumentOutOfRangeException(nameof(gender), gender, null)
            };

            /// <summary>
            /// Generates a random Norwegian full name.
            /// </summary>
            /// <returns>A random Norwegian full name.</returns>
            public string GenerateFullName() => GenerateFullName(Random.NextGender());

            /// <summary>
            /// Generates a random Norwegian full name for the specified <paramref name="gender"/>.
            /// </summary>
            /// <returns>A random Norwegian full name for specified <paramref name="gender"/>.</returns>
            public string GenerateFullName(Gender gender) => $"{GenerateGivenName(gender)} {GenerateFamilyName()}";

            private string GetName(string[] names, double multipleNamesPercent, double hyphenatedPercent)
            {
                if (!UseMultipleNames(multipleNamesPercent, hyphenatedPercent, out var hyphenated))
                {
                    return GetName(names);
                }

                var first = GetName(names);

                if (first.Contains("-"))
                {
                    return first;
                }

                var second = GetName(names);

                if (second.Contains("-"))
                {
                    return second;
                }

                var separator = hyphenated ? '-' : ' ';

                return $"{first}{separator}{second}";

            }

            private string GetName(string[] names) => names[Random.Next(names.Length)];

            private bool UseMultipleNames(double percent, double hyphenatedPercent, out bool hyphenated)
            {
                var number = Random.NextDouble() * 100;

                if (number < hyphenatedPercent)
                {
                    hyphenated = true;
                    return true;
                }

                if (number < percent)
                {
                    hyphenated = false;
                    return true;
                }

                hyphenated = false;
                return false;
            }
        }
    }
}
