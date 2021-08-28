using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Services;

namespace BankSystem.Models
{
    public class UAH : Currency
    {
        public UAH()
        {
            Sign = "UAH";
            Rate = CurrencyAPIService.GetStaticCurrencyRate($"USD{Sign}");
            
        }
    }
}
