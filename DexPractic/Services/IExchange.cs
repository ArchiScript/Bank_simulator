using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;

namespace BankSystem.Services
{
    public interface IExchange
    {
        decimal Calc { get; set; }
        public decimal ConvertCurrency<T>(decimal ammount, T convertFrom, T convertTo) where T: Currency;


    }
}

