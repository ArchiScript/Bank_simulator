using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;

namespace BankSystem.Services
{
    public class Exchange : IExchange
    {

        public decimal Calc { get; set; }
        public decimal ConvertCurrency<T>(decimal ammount, T convertFrom, T convertTo) where T : Currency
        {
            Calc = 0;
            if (convertFrom is USD)
            {
                Calc = ammount * convertFrom.Rate;
            }
            else if (!(convertFrom is USD) && !(convertTo is USD))
            {
                Calc = ammount / convertFrom.Rate * convertTo.Rate;
            }
            else
            {
                Calc = ammount / convertFrom.Rate;
            }

            return Math.Round(Calc, 2);
        }
    }
}
