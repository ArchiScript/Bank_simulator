using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;

namespace BankSystem.Services
{
    public interface IExchange
    {
        public decimal Calc { get; set; }
        public decimal ConvertCurrency() ;


}

