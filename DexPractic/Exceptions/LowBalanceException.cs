using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Exceptions
{
    public class LowBalanceException : Exception
    {
        public LowBalanceException(string message) : base(message)
        {
        }
    }
}
