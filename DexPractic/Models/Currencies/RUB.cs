using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Services;

namespace BankSystem.Models
{
    public class RUB : Currency
    {
        public RUB()
        {
            Rate = 16.3m;
            Sign = "RUB";
        }
    }
}
