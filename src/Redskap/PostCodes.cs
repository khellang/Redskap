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
        public static IReadOnlyCollection<PostCodeInfo> GetAll() => Map.Select(p => new PostCodeInfo(p.Key, p.Value)).ToArray();

        /// <summary>
        /// Checks whether the specified <paramref name="postCode"/> is valid and in use.
        /// </summary>
        /// <param name="postCode">The post code to validate.</param>
        /// <returns><see langword="true"/> if the post code is valid; otherwise, <see langword="false"/>.</returns>
        public static bool IsValid(string postCode)
        {
            if (postCode is null)
            {
                throw new ArgumentNullException(nameof(postCode));
            }

            return short.TryParse(postCode, out var postCodeNumber)
                && Map.ContainsKey(postCodeNumber);
        }

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

            postalName = default;
            return short.TryParse(postCode, out var postCodeNumber)
                && Map.TryGetValue(postCodeNumber, out postalName);
        }
    }
}
