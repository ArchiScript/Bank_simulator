using System;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bankServ = new BankServices();
            var rand = new Random();

            for (int i = 1; i <= 10; i++)
            {
                bankServ.Add(new Client
                {
                    Name = $"Имя{i} Фамилия{i} Отчество{i}",
                    PassNumber = $"I-ПР01234{i}",
                    DateOfBirth = $"{rand.Next(1, 30)}.{rand.Next(1, 12)}.{rand.Next(1955, 2002)}",
                    Id = i,
                });

                ulong uli = (ulong)i;
                bankServ.AddAccount(new Client
                {
                    Name = $"Имя1{i} Фамилия1{i} Отчество1{i}",
                    PassNumber = $"I-ПР01234{i}",
                    DateOfBirth = $"{rand.Next(1, 30)}.{rand.Next(1, 12)}.{rand.Next(1955, 2002)}",
                    Id = i,
                }, new Account { AccNumber = 1000000 + uli, Balance = 4500 + i, CurrencyType = new USD() });
            }

            var testcl = BankServices.clients;
            var testClDict = BankServices.clientsDict;

            //Console.WriteLine(bankServ.Find(BankServices.clients[0]));

            var findp = bankServ.Find<Client>("I-ПР012343");
            Console.WriteLine($"\n IPerson найден по номеру пасспорта: \n {findp.PassNumber} {findp.Name} {findp.DateOfBirth}");

            var testExchange = new Exchange().ConvertCurrency<Currency>(833, new UAH(), new EUR());
            Console.WriteLine($" \n Сконвертировано: {testExchange}");

            var testPair = bankServ.FindFromDict("I-ПР012343");
            foreach (var pair in testPair)

            {
                foreach (var acc in pair.Value)
                {
                    Console.WriteLine($"Найдено из словаря {pair.Key.Name} {acc.AccNumber} {acc.Balance} {acc.CurrencyType.Sign}");
                }

            }

            bankServ.AddAccount(BankServices.clients[0], new Account
            {
                AccNumber = 4524254,
                Balance = 8555,
                CurrencyType = new UAH()
            });
            bankServ.AddAccount(new Client
            {
                Name = "Василий Петрович Петров",
                DateOfBirth = "12.05.1947",
                Id = 888,
                PassNumber = "I-ПР012883"
            }, new Account
            {
                AccNumber = 4533354,
                Balance = 6755,
                CurrencyType = new RUB()
            });
            bankServ.AddAccount(BankServices.clients[0], new Account
            {
                AccNumber = 4577254,
                Balance = 8595,
                CurrencyType = new UAH()
            });

            foreach (var item in testClDict)
            {
                foreach (var ac in item.Value)
                {
                    Console.WriteLine($"{item.Key.Name} {item.Key.PassNumber} {ac.AccNumber} {ac.Balance} {ac.CurrencyType.Sign}");
                }

            }

            var exc = new Exchange();

            var exchangeHandler = new BankServices.ExchangeDelegate(exc.ConvertCurrency);

            //bankServ.MoneyTransfer();

        }


    }
}
