using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Redskap
{
    public static partial class PostCodes
    {
        /// <summary>
        /// Gets all the mapped post codes from Bring's post code register.
        /// </summary>
        /// <returns>All the mapped post codes from Bring's post code register.</returns>
        public static IEnumerable<PostCodeInfo> GetAll() => Map.Select(p => new PostCodeInfo(p.Key, p.Value));

        /// <summary>
        /// Checks whether the specified <paramref name="postCode"/> is valid and in use.
        /// </summary>
        /// <param name="postCode">The post code to validate.</param>
        /// <returns><see langword="true"/> if the post code is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string postCode) => Map.ContainsKey(postCode);

        /// <summary>
        /// Attempts to get a mapped postal name for the specified <paramref name="postCode"/>.
        /// </summary>
        /// <param name="postCode">The post code to find a postal name for.</param>
        /// <param name="postalName">The postal name mapped to the specified <paramref name="postCode"/>.</param>
        /// <returns><see langword="true"/> if a mapped postal name was found; otherwise, <see langword="false"/>.</returns>
        public static bool TryGetPostalName(string postCode, [NotNullWhen(true)] out string? postalName)
        {
            if (postCode is null)
            {
                throw new ArgumentNullException(nameof(postCode));
            }

            if (postCode.Length != 4)
            {
                throw new FormatException($"The post code '{postCode}' has an invalid length. Expected 4 characters.");
            }

            return Map.TryGetValue(postCode, out postalName);
        }
    }
}
