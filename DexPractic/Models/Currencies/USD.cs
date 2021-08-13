using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public class USD : Currency
    {
        public USD()
        {
           Rate = 1.0m;
           Sign = "USD";
        }
    }
}
