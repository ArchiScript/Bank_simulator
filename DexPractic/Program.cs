using System;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Services.BankServices.Add(new Client { 
                Name = "Василий Александрович Петров", 
                PassNumber = "I-ПР012345", 
                DateOfBirth = "25.05.1975" , 
                Id = 0001});




            var testcl = Services.BankServices.clients;
            foreach (var item in testcl)
            {
                Console.WriteLine($"{item.Id}{item.Name}{item.PassNumber}{item.DateOfBirth}");
            }
        }
       
        
    }
}
