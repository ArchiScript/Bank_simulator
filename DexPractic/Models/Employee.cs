using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public class Employee : Person
    {
        public string DateOfEmployment { get; set; }
        public string Position { get; set; }
        public ulong Id { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Employee))
            {
                return false;
            }
            Employee result = (Employee)obj;
            return result.Name == Name && result.PassNumber == PassNumber &&
                result.DateOfBirth == DateOfBirth && result.DateOfEmployment == DateOfEmployment &&
                result.Position == Position && result.Id == Id;
        }
        public static bool operator ==(Employee first, Employee second)
        {
            return first.Equals(second);
        }
        public static bool operator !=(Employee first, Employee second)
        {
            return !first.Equals(second);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + PassNumber.GetHashCode() +
                DateOfBirth.GetHashCode() + DateOfEmployment.GetHashCode() +
                Position.GetHashCode() + Id.GetHashCode();
        }
    }
}
