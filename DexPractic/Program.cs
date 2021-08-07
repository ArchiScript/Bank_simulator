using System;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            BankServices.Add(new Client
            {
                Name = "Василий Александрович Петров",
                PassNumber = "I-ПР012345",
                DateOfBirth = "25.05.1975",
                Id = 0001,
                ClientAccount = 0230000000143456,

            });
            BankServices.Add(new Client
            {
                Name = "Егор Борисович Брынза",
                PassNumber = "I-ПР058845",
                DateOfBirth = "01.02.1963",
                Id = 0002,
                ClientAccount = 0230000000178456,

            });
            BankServices.Add(new Client
            {
                Name = "Алиса Макаровна Шашли",
                PassNumber = "I-ПР753845",
                DateOfBirth = "05.08.2001",
                Id = 0003,
                ClientAccount = 0230000000278456,

            });

            BankServices.Add(new Employee
            {
                Name = "Ульрих Панкратович Бюгельмайстер",
                PassNumber = "I-ПР777845",
                DateOfBirth = "12.02.1987",
                Id = 0004,
                DateOfEmployment = "24.01.2019",
                Position = "Заместитель директора"
            });

            var testcl = BankServices.clients;
            foreach (var item in testcl)
            {
                Console.WriteLine($" {item.Id} {item.Name} {item.PassNumber} {item.DateOfBirth}");
            }

            //Console.WriteLine(BankServices.Find(BankServices.clients[0]).DateOfBirth);
            //Console.WriteLine(BankServices.Find(BankServices.employees[0]).Name);


           // Console.WriteLine(BankServices.FindClient("I-ПР012345").Name);
           Console.WriteLine(BankServices.Find<Employee>("I-ПР777845").Name);

            var testExchange = new Exchange().ConvertCurrency<Currency>(833, new UAH() , new EUR());
            Console.WriteLine(testExchange);
        }


    }
}
