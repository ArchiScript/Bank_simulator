﻿using System;
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




            var testcl = BankServices.clients;
            foreach (var item in testcl)
            {
                Console.WriteLine($"{item.Id}{item.Name}{item.PassNumber}{item.DateOfBirth}");
            }
            
                //Console.WriteLine(BankServices.Find(BankServices.clients[0]).DateOfBirth);

            Console.WriteLine(BankServices.FindClient("I-ПР012345").Name);
            Console.WriteLine(BankServices.Find<Client>("I-ПР012345"));
        }
       
        
    }
}
