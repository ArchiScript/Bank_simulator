using System;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            /*BankServices.Add(new Client
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

            //var findp = BankServices.Find<Employee>("I-ПР777845");
           Console.WriteLine($"\n IPerson найден по номеру пасспорта: \n {findp.PassNumber} {findp.Name} {findp.DateOfBirth}" );

            var testExchange = new Exchange().ConvertCurrency<Currency>(833, new UAH() , new EUR());
            Console.WriteLine($" \n Сконвертировано: {testExchange}") ;*/

            var bc = new BankServices();
            var exc = new Exchange();
            
            var exchangeHandler = new BankServices.ExchangeDelegate(exc.ConvertCurrency);
           // bc.MoneyTransfer();
        }
        

    }
}
