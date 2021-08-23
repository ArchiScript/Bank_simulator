using System;
using BankSystem.Models;
using BankSystem.Services;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;


namespace BankSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var gg = 1;
            /* var bankServ = new BankServices();

             //Заполнение листа
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

                 // Заполнение словаря
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


             //Тестовое добавление в лист клиента и сотр для проверки исключения по возрасту
             bankServ.Add(new Client
             {
                 Name = "Нина Ивановна Дрыщ",
                 PassNumber = "I-ПР133242",
                 DateOfBirth = "17.05.2002",
                 Id = 19
             });
             bankServ.Add(new Employee
             {
                 Name = $"Денис Ростиславович Бабаев",
                 PassNumber = $"I-ПР032360",
                 DateOfBirth = $"12.05.1975",
                 Id = 20,
                 DateOfEmployment = $"25.08.2016",
                 Position = $"Директор"
             });

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

             // Добавление доп счетов
             bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
             {
                 AccNumber = 1013,
                 Balance = 945,
                 CurrencyType = new UAH()
             });
             bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
             {
                 AccNumber = 1025,
                 Balance = 895,
                 CurrencyType = new MDL()
             });
             bankServ.AddClientAccount(new Client
             {
                 Name = "Василий Петрович Петров",
                 DateOfBirth = "12.05.1947",
                 Id = 21,
                 PassNumber = "I-ПР012883"
             },
             new Account
             {
                 AccNumber = 1012,
                 Balance = 675,
                 CurrencyType = new RUB()
             });



             //Вывод в консоль всех клиентов в словаре
             foreach (var pair in testClDict)
             {
                 if (pair.Value.Count > 1)
                 {
                     Console.WriteLine($"счета клиента {pair.Key.Name}: \n ");
                     foreach (var ac in pair.Value)
                     {
                         Console.WriteLine($"{ac.AccNumber} {ac.Balance} {ac.CurrencyType.Sign}");
                     }
                 }
                 else
                 {
                     foreach (var ac in pair.Value)
                     {
                         Console.WriteLine($"{pair.Key.Name} {pair.Key.PassNumber} {ac.AccNumber}" +
                             $" {ac.Balance} {ac.CurrencyType.Sign}");
                     }
                 }

             }
             Console.WriteLine("\n");

             var exc = new Exchange();

             //Присваиваем Func метод ConvertCurrency предварительно создав экземпляр Exchange
             var funcExcDel = bankServ.funcExc;
             funcExcDel = exc.ConvertCurrency;

             //Создаем переменную делегата и присваиваем ей адрес метода 
             var exchangeHandler = new BankServices.ExchangeDelegate(exc.ConvertCurrency);

             //Найти по номеру пасспорта и вернуть ключ-знач
             var tranferCl = bankServ.FindFromDict("I-ПР012341");
             //Выбрать из ключ-значения список счетов
             var accs = bankServ.GetAccountsFromPair(tranferCl);

             //Перевод со счета на счет, Exchange и Вывод 
             if (accs.Count > 1)
             {
                 Console.WriteLine($" Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                     $" Счет {accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign} \n ");
                 bankServ.MoneyTransfer(120, accs[0], accs[1], exchangeHandler);
                 Console.WriteLine("\n ");
             }
             else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }


             //Перевод со счета на счет,Exchange с помощью Func и Вывод  
             if (accs.Count > 1)
             {
                 Console.WriteLine($"Посредством обобщенного делегата Func: \n Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                     $" Счет {accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign} \n ");

                 //Передаем параметром метода func(то есть присвоенную CurrencyCoverter), 
                 bankServ.MoneyTransferFunc(400, accs[0], accs[1], funcExcDel);
             }
             else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }

             //Вывести данные из файла
             var test = bankServ.GetDictFromFile();
             foreach (var pair in test)
             {
                 Console.WriteLine($"Это данные словаря из файла  {pair.Key.Name} {pair.Key.PassNumber}");
                 foreach (var acc in pair.Value)
                 {
                     Console.WriteLine($"----счет----" +
                         $" {acc.AccNumber} {acc.Balance} {acc.CurrencyType.Sign} ");
                 }
             }
             Console.WriteLine("\n");

             //Проверка пользовательского расширения для int,--- конвертация в TimeSpan
             Console.WriteLine("Конвертация числа в экземпляр класса TimeSpan: ");
             Console.WriteLine(1.Seconds() + 24.Minutes() + 4.Hours() - 13.Minutes());

             var myPers = bankServ.Find<Client>("I-ПР012341");

             Type myType = typeof(Client);
             var myProperties = myType.GetProperties();
             var myMethods = myType.GetMethods();
             //G:\C#Projects\DexPractic_Bank_System\BankSystemFiles
             string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles", "reflectionExport.txt");
              var dataExp = new DataExport();
             var testAc = new Account() { AccNumber = 5456545, Balance = 4500, CurrencyType = new USD()};
             dataExp.ExportProperties(testAc, path);

             //dataExp.ExportProperties(bankServ.GetClientFromDict("I-ПР012341"), path);
             dataExp.ExportProperties(new Employee()
             {
                 Name = "Василий Алибабаевич",
                 DateOfBirth = "15.08.1955",
                 PassNumber = "6595624",
                 DateOfEmployment = "12.09.2020",
                 Id = 74,
                 Position = "Дворник"
             }, path);*/

        }
    }
}
