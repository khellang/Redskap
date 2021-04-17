using System;
using System.Runtime.CompilerServices;

namespace Redskap
{
    /// <summary>
    /// Utility for generating random Norwegian names.
    /// </summary>
    public static class Name
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
            public string GenerateFamilyName() => GetName(FamilyNames.All);

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
                Gender.Female => GetName(FemaleNames.All),
                Gender.Male => GetName(MaleNames.All),
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

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private string GetName(string[] names) => names[Random.Next(names.Length)];
        }
    }
}
