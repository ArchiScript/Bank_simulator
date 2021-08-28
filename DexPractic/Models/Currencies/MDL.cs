using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Services;

namespace BankSystem.Models
{
    public class MDL : Currency
    {
        public MDL()
        {
            Sign = "MDL";
            Rate = CurrencyAPIService.GetStaticCurrencyRate($"USD{Sign}");
        }
    }
}
