using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    interface IPerson
    {
       public string Name { get; set; }
       public string PassNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
