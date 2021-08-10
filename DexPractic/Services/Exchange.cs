using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;

namespace BankSystem.Services
{
    public class Exchange : IExchange
    {

        public decimal ConvertCurrency<T>(decimal ammount, T convertFrom, T convertTo) where T : Currency
        {
            decimal Calc;
            Calc = ammount / convertFrom.Rate * convertTo.Rate;
            return Math.Round(Calc, 2);
        }
    }
}
