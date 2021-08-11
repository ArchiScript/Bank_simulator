using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Exceptions
{
    public class BankAdultException : Exception
    {
        public BankAdultException(string message) : base(message)
        {

        }
    }
}
