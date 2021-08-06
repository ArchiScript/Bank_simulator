using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public class Account
    {
        public Currency CurrencyType;
        public decimal Balance { get; set; }
        public ulong AccNumber { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Account))
            {
                return false;
            }
            Account result = (Account)obj;
            return result.CurrencyType == CurrencyType &&
                result.Balance == Balance
                &&
                result.AccNumber == AccNumber;

        }
        public static bool operator ==(Account first, Account second)
        {
            return first.Equals(second);
        }
        public static bool operator !=(Account first, Account second)
        {
            return !first.Equals(second);
        }
        public override int GetHashCode()
        {
            return CurrencyType.GetHashCode() + Balance.GetHashCode()
               +
               AccNumber.GetHashCode();

        }


    }
}
