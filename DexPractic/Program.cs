﻿using System;
using BankSystem.Models;
using BankSystem.Services;

namespace BankSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            var bankServ = new BankServices();

            //Заполнение словаря
            for (int i = 1; i <= 5; i++)
            {
                var rand = new Random();
                bankServ.Add(new Client
                {
                    Name = $"Имя{i} Фамилия{i} Отчество{i}",
                    PassNumber = $"I-ПР01234{i}",
                    DateOfBirth = $"{rand.Next(1, 30)}.{rand.Next(1, 12)}.{rand.Next(1955, 2002)}",
                    Id = i,
                });

                ulong uli = (ulong)i;
                bankServ.AddClientAccount(new Client
                {
                    Name = $"Имя1{i} Фамилия1{i} Отчество1{i}",
                    PassNumber = $"I-ПР01234{i}",
                    DateOfBirth = $"{rand.Next(1, 30)}.{rand.Next(1, 12)}.{rand.Next(1955, 2002)}",
                    Id = i,
                }, new Account { AccNumber = 1000 + uli, Balance = 730 + i, CurrencyType = new USD() });
            }

            var testClDict = BankServices.clientsDict;

            //Найти из словаря и показать
            var testPair = bankServ.FindFromDict("I-ПР012341");
            foreach (var pair in testPair)
            {
                foreach (var acc in pair.Value)
                {
                    Console.WriteLine($"Найдено из словаря {pair.Key.Name} {acc.AccNumber} {acc.Balance} {acc.CurrencyType.Sign}");
                }
            }

            var testExchange = new Exchange().ConvertCurrency<Currency>(100, new EUR(), new USD());
            Console.WriteLine($" \n Сконвертировано: {testExchange}");

            // Добавление новый счетов
            bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
            {
                AccNumber = 1013,
                Balance = 945,
                CurrencyType = new UAH()
            });
            bankServ.AddClientAccount(new Client
            {
                Name = "Василий Петрович Петров",
                DateOfBirth = "12.05.1947",
                Id = 888,
                PassNumber = "I-ПР012883"
            },
            new Account
            {
                AccNumber = 1012,
                Balance = 675,
                CurrencyType = new RUB()
            });
            bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
            {
                AccNumber = 1025,
                Balance = 895,
                CurrencyType = new MDL()
            });


            //Вывод в консоль всех клиентов в словаре
            foreach (var pair in testClDict)
            {
                foreach (var ac in pair.Value)
                {
                    Console.WriteLine($"{pair.Key.Name} {pair.Key.PassNumber} " +
                        $"{ac.AccNumber} {ac.Balance} {ac.CurrencyType.Sign}");
                }

            }
            Console.WriteLine("\n");

            var exc = new Exchange();

            // Присваиваем переменной делегата адрес метода 
            var exchangeHandler = new BankServices.ExchangeDelegate(exc.ConvertCurrency);

            //Найти по номеру пасспорта и вернуть ключ-знач
            var tranferCl = bankServ.FindFromDict("I-ПР012341");
            //Выбрать из ключ-значения список счетов
            var accs = bankServ.GetAccountsFromPair(tranferCl);

            if (accs.Count > 1)
            {
                Console.WriteLine($"Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                    $" Счет {accs[2].AccNumber} {accs[2].Balance} {accs[2].CurrencyType.Sign} \n ");
                bankServ.MoneyTransfer(120, accs[0], accs[2], exchangeHandler);
            }
            else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }
        }
    }
}
