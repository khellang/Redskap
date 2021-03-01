namespace Redskap
{
    /// <summary>
    /// An enum representing the different types of Norwegian identification numbers.
    /// </summary>
    public enum IdentificationNumberKind
    {
        /// <summary>
        /// A Norwegian national identity number, or F-number, is a unique
        /// identifying number assigned to persons born in Norway.
        /// </summary>
        /// <remarks>
        /// The number consists of 11 digits, of which the first six digits indicate
        /// the person's date of birth.
        /// </remarks>
        FNumber = 0,

        /// <summary>
        /// A D number is a temporary identification number which can be assigned to foreign
        /// persons who'll generally be resident in Norway for less than six months.
        /// </summary>
        /// <remarks>
        /// The number consists of 11 digits, of which the first six digits indicate
        /// the person's date of birth, but the first digit is increased by 4.
        /// </remarks>
        DNumber = 1,

        /// <summary>
        /// An H number is a emergency/temporary identification number assigned to persons
        /// that don't have an F- og D-number, or where this number is unknown. It's typically
        /// used
        /// </summary>
        /// <remarks>
        /// The number consists of 11 digits, of which the first six digits indicate
        /// the person's date of birth, but the third digit is increased by 4.
        /// </remarks>
        HNumber = 2,
    }
}
