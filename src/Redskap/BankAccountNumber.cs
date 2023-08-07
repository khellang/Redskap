using System;

namespace Redskap;

/// <summary>
/// Represents a Norwegian bank account number.
/// The number consists of a <see cref="RegisterNumber"/>, an <see cref="AccountGroup"/> and a <see cref="CustomerNumber"/>,
/// as well as a <see cref="CheckDigit"/>.
/// </summary>
public readonly partial struct BankAccountNumber : IEquatable<BankAccountNumber>
{
    private BankAccountNumber(int registerNumber, int accountGroup, int customerNumber, int checkDigit)
    {
        RegisterNumber = registerNumber;
        AccountGroup = accountGroup;
        CustomerNumber = customerNumber;
        CheckDigit = checkDigit;
    }

    /// <summary>
    /// The bank's register number, i.e. the first four digits of the account
    /// number, identifies both the bank and the branch/department.
    /// </summary>
    public int RegisterNumber { get; }

    /// <summary>
    /// The next two digits are the account group.
    /// The purpose is to identify special account groups internally in the banks,
    /// as transactions to and from such accounts have traditionally been subject
    /// to special treatment in the settlement systems.
    /// Today, this is not so widely used.
    /// </summary>
    public int AccountGroup { get; }

    /// <summary>
    /// The next four digits are the customer number.
    /// This number is unique for each individual customer, and for those banks that use
    /// account groups, it will be the only thing that distinguishes individual customers'
    /// current accounts. The customer number will in some cases be six digits (22 3333),
    /// if the bank does not use account groups.
    /// </summary>
    public int CustomerNumber { get; }

    /// <summary>
    /// The last digit means the check digit and is calculated on the basis of the previous 10 digits.
    /// </summary>
    public int CheckDigit { get; }

    /// <inheritdoc />
    public bool Equals(BankAccountNumber other)
    {
        return RegisterNumber == other.RegisterNumber
               && AccountGroup == other.AccountGroup
               && CustomerNumber == other.CustomerNumber
               && CheckDigit == other.CheckDigit;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is BankAccountNumber other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(RegisterNumber, AccountGroup, CustomerNumber, CheckDigit);
    }

    /// <summary>
    /// Indicates whether the two specified <see cref="BankAccountNumber"/> instances are equal.
    /// </summary>
    /// <param name="left">The left number to compare.</param>
    /// <param name="right">The right number to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left" /> and <paramref name="right"/>
    /// represent the same value; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(BankAccountNumber left, BankAccountNumber right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Indicates whether the two specified <see cref="BankAccountNumber"/> instances are unequal.
    /// </summary>
    /// <param name="left">The left number to compare.</param>
    /// <param name="right">The right number to compare.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="left" /> and <paramref name="right"/>
    /// represent different values; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(BankAccountNumber left, BankAccountNumber right)
    {
        return !left.Equals(right);
    }
}
