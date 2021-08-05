using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
     public class Employee:IPerson
    {
        public string Name { get; set; }
        public string PassNumber { get; set; }
    }
}
