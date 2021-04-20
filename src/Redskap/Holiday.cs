using System;

namespace Redskap
{
    /// <summary>
    /// Represents a Norwegian public holiday.
    /// </summary>
    public readonly struct Holiday : IEquatable<Holiday>, IComparable<Holiday>, IComparable
    {
        /// <summary>
        /// Creates a new instance of <see cref="Holiday"/> based on <paramref name="year"/>,
        /// <paramref name="month"/> and <paramref name="day"/>.
        /// </summary>
        /// <param name="year">The year of the holiday.</param>
        /// <param name="month">The month of the holiday.</param>
        /// <param name="day">The day of the holiday.</param>
        /// <param name="norwegianName">The Norwegian name of the holiday.</param>
        /// <param name="englishName">The English name of the holiday.</param>
        public Holiday(int year, int month, int day, string norwegianName, string englishName)
            : this(new DateTime(year, month, day), norwegianName, englishName)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="Holiday"/> based on a <paramref name="date"/>.
        /// </summary>
        /// <param name="date">The date of the holiday.</param>
        /// <param name="norwegianName">The Norwegian name of the holiday.</param>
        /// <param name="englishName">The English name of the holiday.</param>
        public Holiday(DateTime date, string norwegianName, string englishName)
        {
            Date = date;
            NorwegianName = norwegianName;
            EnglishName = englishName;
        }

        /// <summary>
        /// Gets the holiday's date.
        /// </summary>
        public DateTime Date { get; }

        /// <summary>
        /// Gets the holiday's Norwegian name.
        /// </summary>
        public string NorwegianName { get; }

        /// <summary>
        /// Gets the holiday's English name.
        /// </summary>
        public string EnglishName { get; }

        /// <inheritdoc />
        public int CompareTo(Holiday other) => Date.CompareTo(other.Date);

        /// <inheritdoc />
        public int CompareTo(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return 1;
            }

            if (obj is Holiday other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Object must be of type {nameof(Holiday)}");
        }

        /// <inheritdoc />
        public bool Equals(Holiday other) => Date.Equals(other.Date);

        /// <inheritdoc />
        public override bool Equals(object? obj) => obj is Holiday other && Equals(other);

        /// <inheritdoc />
        public override int GetHashCode() => Date.GetHashCode();

#pragma warning disable 1591
        public static bool operator ==(Holiday left, Holiday right) => left.Equals(right);

        public static bool operator !=(Holiday left, Holiday right) => !left.Equals(right);

        public static bool operator <(Holiday left, Holiday right) => left.CompareTo(right) < 0;

        public static bool operator >(Holiday left, Holiday right) => left.CompareTo(right) > 0;

        public static bool operator <=(Holiday left, Holiday right) => left.CompareTo(right) <= 0;

        public static bool operator >=(Holiday left, Holiday right) => left.CompareTo(right) >= 0;
#pragma warning restore 1591

        /// <inheritdoc />
        public override string ToString() => NorwegianName;
    }
}
