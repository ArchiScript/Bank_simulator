using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public class UAH : Currency
    {
        public UAH()
        {
            Rate = 26.8m;
            Sign = "UAH";
        }
    }
}
