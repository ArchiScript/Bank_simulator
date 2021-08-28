using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Services;

namespace BankSystem.Models
{
    public class USD : Currency
    {
        public USD()
        {
            Sign = "USD";
            Rate = 1.0m;
            
        }
    }
}
