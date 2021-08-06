using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public class Person:IPerson
    {
        public string Name { get; set; }
        public string PassNumber { get; set; }
        public string DateOfBirth { get; set; }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Person))
            {
                return false;
            }
            Person result = (Person)obj;
            return result.Name == Name && result.PassNumber == PassNumber &&
                result.DateOfBirth == DateOfBirth;
        }
        public static bool operator ==(Person first, Person second)
        {
            return first.Equals(second);
        }
        public static bool operator !=(Person first, Person second)
        {
            return !first.Equals(second);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode() + PassNumber.GetHashCode() +
                DateOfBirth.GetHashCode() ;
        }
    }
}
