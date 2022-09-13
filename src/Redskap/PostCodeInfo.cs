using System;
using System.Globalization;

namespace Redskap
{
    /// <summary>
    /// Represents information about a Norwegian post code from Bring's post code register.
    /// </summary>
    public readonly struct PostCodeInfo : IEquatable<PostCodeInfo>
    {
        /// <summary>
        /// We store the postcodes as integers internally for space and time efficiency.
        /// The number is then converted to a (padded) string lazily.
        /// </summary>
        internal PostCodeInfo(short postCodeNumber, string postalName)
        {
            PostCodeNumber = postCodeNumber;
            PostalName = postalName;
        }

        /// <summary>
        /// The post code value, i.e. 0105.
        /// </summary>
        public string PostCode => PostCodeNumber.ToString("0000", CultureInfo.InvariantCulture);

        /// <summary>
        /// The post code's postal name, i.e. OSLO.
        /// </summary>
        public string PostalName { get; }

        private short PostCodeNumber { get; }

        /// <summary>
        /// Deconstructs the post code info into the <paramref name="postCode"/>
        /// and <paramref name="postalName"/> components.
        /// </summary>
        /// <param name="postCode">The post code.</param>
        /// <param name="postalName">The post code's postal name.</param>
        public void Deconstruct(out string postCode, out string postalName) =>
            (postCode, postalName) = (PostCode, PostalName);

        /// <inheritdoc />
        public bool Equals(PostCodeInfo other) => PostCodeNumber == other.PostCodeNumber;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is PostCodeInfo other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => PostCodeNumber.GetHashCode();

#pragma warning disable 1591
        public static bool operator ==(PostCodeInfo left, PostCodeInfo right) => left.Equals(right);

        public static bool operator !=(PostCodeInfo left, PostCodeInfo right) => !left.Equals(right);
#pragma warning restore 1591

        /// <inheritdoc />
        public override string ToString() => $"{PostCode} {PostalName}";
    }
}
