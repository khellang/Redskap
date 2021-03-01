using System;

namespace Redskap
{
    /// <summary>
    /// Represents a Norwegian identification number of a specific <see cref="Kind"/>.
    /// The number consists of a <see cref="DateOfBirth"/>, an <see cref="IndividualNumber"/> and a <see cref="Gender"/>.
    /// </summary>
    public readonly partial struct IdentificationNumber : IEquatable<IdentificationNumber>
    {
        private IdentificationNumber(DateTime dateOfBirth, int individualNumber, IdentificationNumberKind kind)
        {
            DateOfBirth = dateOfBirth;
            IndividualNumber = individualNumber;
            Kind = kind;
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
        public IdentificationNumberKind Kind { get; }

        /// <summary>
        /// The <see cref="Gender"/> represented by this
        /// </summary>
        public Gender Gender => GetGender(IndividualNumber);

        /// <inheritdoc />
        public bool Equals(IdentificationNumber other)
        {
            return DateOfBirth.Equals(other.DateOfBirth)
                && IndividualNumber == other.IndividualNumber
                && Kind == other.Kind;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return obj is IdentificationNumber other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(DateOfBirth, IndividualNumber, Kind);
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

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Gender} {IndividualNumber} born {DateOfBirth:d}";
        }

        private static Gender GetGender(int individual)
        {
            return individual % 2 == 0 ? Gender.Female : Gender.Male;
        }
    }
}
