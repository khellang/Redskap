namespace Redskap
{
    /// <summary>
    /// Represents informatin about a Norwegian post code from Bring's post code register.
    /// </summary>
    public readonly struct PostCodeInfo
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
    }
}
