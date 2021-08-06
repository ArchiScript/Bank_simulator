using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Models
{
    public interface IPerson
    {
       public string Name { get; set; }
       public string PassNumber { get; set; }
        public string DateOfBirth { get; set; }
        
    }
}
