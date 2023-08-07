using System;
using static Redskap.FormattingHelpers;

namespace Redskap;

public readonly partial struct BankAccountNumber
{
    /// <inheritdoc />
    public override string ToString()
    {
        const int length = Length + 2;

#if NETSTANDARD
        var buffer = new char[length];

        Format(this, buffer);

        return new string(buffer);
#else
        return string.Create(length, this, (span, number) =>
        {
            Format(number, span);
        });
#endif
    }

    private static void Format(BankAccountNumber number, Span<char> buffer)
    {
        WriteDigit((uint) number.CheckDigit, buffer, 12);
        WriteFourDecimalDigits((uint) number.CustomerNumber, buffer, 8);
        buffer[7] = ' ';
        WriteTwoDecimalDigits((uint) number.AccountGroup, buffer, 5);
        buffer[4] = ' ';
        WriteFourDecimalDigits((uint) number.RegisterNumber, buffer, 0);
    }
}
