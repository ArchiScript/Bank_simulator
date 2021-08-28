﻿using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Services;

namespace BankSystem.Models
{
    public class EUR : Currency
    {
        public EUR()
        {
            Sign = "EUR";
            Rate = CurrencyAPIService.GetStaticCurrencyRate($"USD{Sign}");

        }
    }
}
