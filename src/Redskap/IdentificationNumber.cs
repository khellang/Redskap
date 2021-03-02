using System;

namespace Redskap
{
    /// <summary>
    /// Represents a Norwegian identification number of a specific <see cref="Kind"/>.
    /// The number consists of a <see cref="DateOfBirth"/>, an <see cref="IndividualNumber"/> and a <see cref="Gender"/>.
    /// </summary>
    public readonly partial struct IdentificationNumber : IEquatable<IdentificationNumber>
    {
        private IdentificationNumber(DateTime dateOfBirth, int individualNumber, int checkDigits, Kind kind)
        {
            DateOfBirth = dateOfBirth;
            IndividualNumber = individualNumber;
            CheckDigits = checkDigits;
            NumberKind = kind;
        }

        /// <summary>
        /// The date of birth represented by this
        /// identification number, in local time.
        /// </summary>
        public DateTime DateOfBirth { get; }

        /// <summary>
        /// The individual number. This number is allocated
        /// sequentially within the specific <see cref="DateOfBirth"/>.
        /// </summary>
        public int IndividualNumber { get; }

        /// <summary>
        /// The identification number kind.
        /// </summary>
        public Kind NumberKind { get; }

        /// <summary>
        /// The <see cref="Gender"/> represented by this
        /// </summary>
        public Gender Gender => GetGender(IndividualNumber);

        private int CheckDigits { get; }

        /// <inheritdoc />
        public bool Equals(IdentificationNumber other)
        {
            return DateOfBirth.Equals(other.DateOfBirth)
                && IndividualNumber == other.IndividualNumber
                && NumberKind == other.NumberKind;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is IdentificationNumber other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(DateOfBirth, IndividualNumber, NumberKind);
        }

        /// <summary>
        /// Indicates whether the two specified <see cref="IdentificationNumber"/> instances are equal.
        /// </summary>
        /// <param name="left">The left number to compare.</param>
        /// <param name="right">The right number to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left" /> and <paramref name="right"/>
        /// represent the same value; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(IdentificationNumber left, IdentificationNumber right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Indicates whether the two specified <see cref="IdentificationNumber"/> instances are unequal.
        /// </summary>
        /// <param name="left">The left number to compare.</param>
        /// <param name="right">The right number to compare.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="left" /> and <paramref name="right"/>
        /// represent different values; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(IdentificationNumber left, IdentificationNumber right)
        {
            return !left.Equals(right);
        }

        private static Gender GetGender(int individual)
        {
            return individual % 2 == 0 ? Gender.Female : Gender.Male;
        }

        /// <summary>
        /// An enum representing the different types of Norwegian identification numbers.
        /// </summary>
        public enum Kind
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
}
