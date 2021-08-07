using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public class Client : Person

    {
        public ulong Id { get; set; }
        //public List<Account> Accounts = new List<Account>();
        public ulong ClientAccount { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Client))
            {
                return false;
            }
            Client result = (Client)obj;
            return result.Name == Name && result.PassNumber == PassNumber
                && result.DateOfBirth == DateOfBirth && result.Id == Id &&
                result.ClientAccount == ClientAccount;
        }
        public static bool operator ==(Client first, Client second)
        {
            return first.Equals(second);
        }
        public static bool operator !=(Client first, Client second)
        {
            return !first.Equals(second);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + PassNumber.GetHashCode()
                + DateOfBirth.GetHashCode() + Id.GetHashCode() + ClientAccount.GetHashCode();
        }
    }


}

