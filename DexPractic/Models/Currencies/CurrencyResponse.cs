using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models.Currencies
{
    public class CurrencyResponse
    {
        public bool Success { get; set; }
        public string Terms { get; set; }
        public string Privacy { get; set; }
        public int Timestamp { get; set; }
        public string Source { get; set; }
        public Dictionary<string, decimal> Quotes { get; set; }
    }
}
