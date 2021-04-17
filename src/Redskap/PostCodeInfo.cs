using System;

namespace Redskap
{
    /// <summary>
    /// Represents informatin about a Norwegian post code from Bring's post code register.
    /// </summary>
    public readonly struct PostCodeInfo : IEquatable<PostCodeInfo>
    {
        /// <summary>
        /// Creates a new instance of a <see cref="PostCodeInfo"/>.
        /// </summary>
        /// <param name="postCode">The post code.</param>
        /// <param name="postalName">The post code's postal name.</param>
        public PostCodeInfo(string postCode, string postalName)
        {
            PostCode = postCode;
            PostalName = postalName;
        }

        /// <summary>
        /// The post code value, i.e. 0105.
        /// </summary>
        public string PostCode { get; }

        /// <summary>
        /// The post code's postal name, i.e. OSLO.
        /// </summary>
        public string PostalName { get; }

        /// <summary>
        /// Deconstructs the post code info into the <paramref name="postCode"/>
        /// and <paramref name="postalName"/> components.
        /// </summary>
        /// <param name="postCode">The post code.</param>
        /// <param name="postalName">The post code's postal name.</param>
        public void Deconstruct(out string postCode, out string postalName) =>
            (postCode, postalName) = (PostCode, PostalName);

        /// <inheritdoc />
        public bool Equals(PostCodeInfo other) => PostCode == other.PostCode;

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is PostCodeInfo other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => PostCode.GetHashCode();

        /// <summary>
        /// Compares two instances of <see cref="PostCodeInfo"/> for equality.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><see langword="true"/> if the post codes are equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(PostCodeInfo left, PostCodeInfo right) => left.Equals(right);

        /// <summary>
        /// Compares two instances of <see cref="PostCodeInfo"/> for inequality.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><see langword="true"/> if the post codes are not equal; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(PostCodeInfo left, PostCodeInfo right) => !left.Equals(right);

        /// <inheritdoc />
        public override string ToString() => $"{PostCode} {PostalName}";
    }
}
