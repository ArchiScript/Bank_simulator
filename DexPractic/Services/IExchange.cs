using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Services
{
    public interface IExchange
    {
        decimal Calc { get; set; }
        public decimal ConvertCurrency();
    }
}
