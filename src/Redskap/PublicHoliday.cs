using System;

namespace Redskap
{
    /// <summary>
    /// Represents a Norwegian public holiday.
    /// </summary>
    public readonly struct PublicHoliday : IEquatable<PublicHoliday>, IComparable<PublicHoliday>, IComparable
    {
        /// <summary>
        /// Creates a new instance of <see cref="PublicHoliday"/> based on <paramref name="year"/>,
        /// <paramref name="month"/> and <paramref name="day"/>.
        /// </summary>
        /// <param name="year">The year of the holiday.</param>
        /// <param name="month">The month of the holiday.</param>
        /// <param name="day">The day of the holiday.</param>
        /// <param name="localName">The Norwegian name of the holiday.</param>
        /// <param name="name">The English name of the holiday.</param>
        public PublicHoliday(int year, int month, int day, string name, string localName)
            : this(new DateTime(year, month, day), name, localName)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="PublicHoliday"/> based on a <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date of the holiday.</param>
        /// <param name="localName">The Norwegian name of the holiday.</param>
        /// <param name="name">The English name of the holiday.</param>
        public PublicHoliday(DateTime date, string name, string localName)
        {
            Date = date;
            Name = name;
            LocalName = localName;
        }

        /// <summary>
        /// Used internally for equoality checking against other <see cref="PublicHoliday"/>
        /// instances. <see cref="Name"/> and <see cref="LocalName"/> is never used.
        /// </summary>
        /// <param name="date"></param>
        internal PublicHoliday(DateTime date) : this(date, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Gets the holiday's date.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets the holiday's Norwegian name.
        /// </summary>
        public string LocalName { get; }

        /// <summary>
        /// Gets the holiday's English name.
        /// </summary>
        public string Name { get; }

        /// <inheritdoc />
        public int CompareTo(PublicHoliday other) => Date.CompareTo(other.Date);

        /// <inheritdoc />
        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (obj is PublicHoliday other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Object must be of type {nameof(PublicHoliday)}");
        }

        /// <inheritdoc />
        public bool Equals(PublicHoliday other) => Date.Equals(other.Date);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is PublicHoliday other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => Date.GetHashCode();

#pragma warning disable 1591
        public static bool operator ==(PublicHoliday left, PublicHoliday right) => left.Equals(right);

        public static bool operator !=(PublicHoliday left, PublicHoliday right) => !left.Equals(right);

        public static bool operator <(PublicHoliday left, PublicHoliday right) => left.CompareTo(right) < 0;

        public static bool operator >(PublicHoliday left, PublicHoliday right) => left.CompareTo(right) > 0;

        public static bool operator <=(PublicHoliday left, PublicHoliday right) => left.CompareTo(right) <= 0;

        public static bool operator >=(PublicHoliday left, PublicHoliday right) => left.CompareTo(right) >= 0;
#pragma warning restore 1591

        /// <inheritdoc />
        public override string ToString() => Name;
    }
}
