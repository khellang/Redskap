using System;
using static Redskap.FormattingHelpers;

namespace Redskap
{
    public readonly partial struct IdentificationNumber
    {
        /// <inheritdoc />
        public override string ToString()
        {
#if NETSTANDARD
            var buffer = new char[Length];

            Format(this, buffer);

            return new string(buffer);
#else
            return string.Create(Length, this, (span, number) =>
            {
                Format(number, span);
            });
#endif
        }

        private static void Format(IdentificationNumber number, Span<char> buffer)
        {
            WriteTwoDecimalDigits((uint) number.CheckDigits, buffer, 9);
            WriteThreeDecimalDigits((uint) number.IndividualNumber, buffer, 6);
            WriteDateOfBirth(buffer, number.DateOfBirth, number.NumberKind);
        }

        private static void WriteDateOfBirth(Span<char> destination, DateTime dateOfBirth, Kind kind)
        {
            WriteYear(destination, dateOfBirth);
            WriteMonth(destination, dateOfBirth, kind);
            WriteDay(destination, dateOfBirth, kind);
        }

        private static void WriteDay(Span<char> destination, DateTime dateOfBirth, Kind kind)
        {
            var day = dateOfBirth.Day;

            if (kind == Kind.DNumber)
            {
                day += 40;
            }

            WriteTwoDecimalDigits((uint) day, destination, 0);
        }

        private static void WriteMonth(Span<char> destination, DateTime dateOfBirth, Kind kind)
        {
            var month = dateOfBirth.Month;

            if (kind == Kind.HNumber)
            {
                month += 40;
            }

            WriteTwoDecimalDigits((uint) month, destination, 2);
        }

        private static void WriteYear(Span<char> destination, DateTime dateOfBirth)
        {
            WriteTwoDecimalDigits((uint) dateOfBirth.Year % 100, destination, 4);
        }
    }
}
