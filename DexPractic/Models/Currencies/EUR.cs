using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Services;

namespace BankSystem.Models
{
    public class EUR : Currency
    {
        public EUR()
        {
            Rate = 0.84m;
            Sign = "EUR";
        }
    }
}
