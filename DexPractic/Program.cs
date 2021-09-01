using System;
using BankSystem.Models;
using BankSystem.Services;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BankSystem.Models.Currencies;
using System.Threading;


namespace BankSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var bankServ = new BankServices();
           // var statAccs = bankServ.AccountsFromFileToDict();
            var dictFromFile = bankServ.GetDictFromFile();
            //======================== Заполнение листа ============================

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

                //============================= Заполнение словаря ==================
                ulong uli = (ulong)i;
                bankServ.AddClientAccount(new Client
                {
                    Name = $"Имя1{i} Фамилия1{i} Отчество1{i}",
                    PassNumber = $"I-ПР01234{i}",
                    DateOfBirth = $"{rand.Next(1, 30)}.{rand.Next(1, 12)}.{rand.Next(1955, 2002)}",
                    Id = i,
                }, new Account { AccNumber = 1000 + uli, Balance = 730 + i, CurrencyType = new USD() });
            }

            


            //================== Тестовое добавление в лист клиента и сотр
            //для проверки исключения по возрасту ===================================
            /*  bankServ.Add(new Client
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
              });*/

            //==============  Найти из словаря и показать ==================================

            /*var testPair = bankServ.FindFromDict("I-ПР012341");
            foreach (var pair in testPair)
            {
                foreach (var acc in pair.Value)
                {
                    Console.WriteLine($"Найдено из словаря {pair.Key.Name} {acc.AccNumber} {acc.Balance} {acc.CurrencyType.Sign}");
                }
            }

            var testExchange = new Exchange().ConvertCurrency<Currency>(0, new EUR(), new USD());
            Console.WriteLine($" \n Сконвертировано: {testExchange}");
*/
            // =======================  Добавление доп счетов ================================

            bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
            {
                AccNumber = 1013,
                Balance = 0,
                CurrencyType = new UAH()
            });
            bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
            {
                AccNumber = 1025,
                Balance = 19895,
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

            bankServ.AddClientAccount(bankServ.GetClientFromDict("I-ПР012341"), new Account
            {
                AccNumber = 1016,
                Balance = 900,
                CurrencyType = new EUR()
            });

            //=========================== Вывод в консоль всех клиентов в словаре =================

            /*foreach (var pair in dictFromFile)
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
            Console.WriteLine("\n");*/


            var exc = new Exchange();

            //Присваиваем Func метод ConvertCurrency предварительно создав экземпляр Exchange
            var funcExcDel = bankServ.funcExc;
            funcExcDel = exc.ConvertCurrency;

            //Создаем переменную делегата и присваиваем ей адрес метода 
            var exchangeHandler = new BankServices.ExchangeDelegate(exc.ConvertCurrency);

            //Найти по номеру пасспорта и вернуть ключ-знач
            var transferCl = bankServ.FindFromDict("I-ПР012341");
            //Выбрать из ключ-значения список счетов
            //var accs = bankServ.GetAccountsFromPair(transferCl);
            var accs = bankServ.GetAccountsFromFile("I-ПР012341");
            //======================== Перевод со счета на счет, Exchange и Вывод ================

            /*if (accs.Count > 1)
            {
                Console.WriteLine($" Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                    $" Счет {accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign} \n ");
                bankServ.MoneyTransfer(120, accs[0], accs[1], exchangeHandler);
                Console.WriteLine("\n ");
            }
            else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }*/


            //===================== Перевод со счета на счет,Exchange с помощью Func и Вывод  ====================

            /* if (accs.Count > 1)
             {
                 Console.WriteLine($"Посредством обобщенного делегата Func: \n Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                     $" Счет {accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign} \n ");

                 //Передаем параметром метода func(то есть присвоенную CurrencyCoverter), 
                 bankServ.MoneyTransferFunc(400, accs[0], accs[1], funcExcDel);
             }
             else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }*/

            //====================== Вывести данные из файла  ===========================
            /* var test = bankServ.GetDictFromFile();
             foreach (var pair in test)
             {
                 Console.WriteLine($"Это данные словаря из файла  {pair.Key.Name} {pair.Key.PassNumber}");
                 foreach (var acc in pair.Value)
                 {
                     Console.WriteLine($"----счет----" +
                         $" {acc.AccNumber} {acc.Balance} {acc.CurrencyType.Sign} ");
                 }
             }
             Console.WriteLine("\n");*/

            //===========Проверка пользовательского расширения для int,--- конвертация в TimeSpan ========

            /*Console.WriteLine("Конвертация числа в экземпляр класса TimeSpan: ");
            Console.WriteLine(1.Seconds() + 24.Minutes() + 4.Hours() - 13.Minutes());

            var myPers = bankServ.Find<Client>("I-ПР012341");*/

            //============================== REFLECTION ===================================
            // DATA EXPORT
            /* Type myType = typeof(Client);
             var myProperties = myType.GetProperties();
             var myMethods = myType.GetMethods();
             //G:\C#Projects\DexPractic_Bank_System\BankSystemFiles
             string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles", "reflectionExport.txt");
             var dataExp = new DataExport();
             var testAc = new Account() { AccNumber = 5456545, Balance = 4500, CurrencyType = new USD() };

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
             }, path);
 */
            //========================   ОГРАНИЧЕННОЕ ЧИСЛО ЗАПРОСОВ К API ================

            /*var currencyApi = new CurrencyAPIService();
            CurrencyResponse myCurrencyData = await currencyApi.GetCurrencies();



            var mycur = currencyApi.GetCurrencyRate("USDRUB");
            Console.WriteLine(mycur);
            var myCurrencyDataFromFile = currencyApi.GetCurrencyResponseFromFile();
            var timeStamp = currencyApi.GetCurrencyResponseDate();
            Console.WriteLine(timeStamp);
            var myUah = new UAH();
            Console.WriteLine(myUah.Rate);*/

            //==================================Threading================================

            /*bankServ.ShowClients();
            object locker = new object();

            ThreadPool.QueueUserWorkItem(_ =>
            {
                int hash = Thread.CurrentThread.GetHashCode();
                while (true)
                {
                    lock (locker)
                    {
                        bankServ.ShowClients();
                        Console.WriteLine($"Вывод Поток --- {hash}");

                    }
                    Thread.Sleep(1000);

                }
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {
                int hash = Thread.CurrentThread.GetHashCode();
                for (int i = 0; i < 10; i++)
                {
                    lock (locker)
                    {
                        var rand = new Random();

                        bankServ.Add(new Client
                        {
                            Name = $"Добавление Поток ---- {hash} Имя{i} Фамилия{i} Отчество{i}",
                            PassNumber = $"I-ПР01234{i}",
                            DateOfBirth = $"{rand.Next(1, 30)}.{rand.Next(1, 12)}.{rand.Next(1955, 2002)}",
                            Id = i,
                        });
                    }

                    Thread.Sleep(1000);

                }
            });


            Console.ReadLine();*/


            //====================== THREADING MONEY TRANSFER =======================
            //Проверить состояние счета в гривнах
            /*var uah = new UAH();
            Console.WriteLine(uah.Rate);
            *//*Console.WriteLine($"{accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n " +
                $"{accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign}");*//*

            Console.WriteLine((bankServ.GetAccountsFromFile("I-ПР012341")[0].Balance));

            object locker1 = new object();


            ThreadPool.QueueUserWorkItem(_ =>
            {
                var hash = Thread.CurrentThread.GetHashCode();
                lock (locker1)
                {
                    if (accs.Count > 1)
                    {
                        //Перевод со счета на счет, Exchange и Вывод 
                        Console.WriteLine($"------- Поток {hash} ---- Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                           $" Счет {accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign} \n ");
                        bankServ.MoneyTransfer(100, accs[0], accs[1], exchangeHandler);
                        Console.WriteLine("\n ");
                    }
                    else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }
                }
                // Thread.Sleep(500);
            });


            ThreadPool.QueueUserWorkItem(_ =>
            {
                var hash = Thread.CurrentThread.GetHashCode();
                lock (locker1)
                {
                    if (accs.Count > 1)
                    {
                        //Перевод со счета на счет, Exchange и Вывод 
                        Console.WriteLine($" ------- Поток {hash} ---- Счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign}  " +
                           $" Счет {accs[1].AccNumber} {accs[1].Balance} {accs[1].CurrencyType.Sign} \n ");
                        bankServ.MoneyTransfer(100, accs[0], accs[1], exchangeHandler); int result = (int)Math.Round(accs[1].Balance);
                        Console.WriteLine($"\n { result} ");

                    }
                    else { Console.WriteLine($"Найден только один счет {accs[0].AccNumber} {accs[0].Balance} {accs[0].CurrencyType.Sign} \n "); }
                }

                //Thread.Sleep(1000);
            });

            //Console.ReadLine();
            Console.WriteLine($"---------{ accs[1].AccNumber} { accs[1].Balance} { accs[1].CurrencyType.Sign} \n");

            var cl = bankServ.GetClientFromDict("I-ПР012341");
            Console.WriteLine($"{cl.Name}");

            bankServ.ShowClients();
            var a = bankServ.PutMoney(523, accs[0]);
            Console.WriteLine(a.Balance + accs[0].CurrencyType.Sign);*/


            //var statAccs = BankServices.clPassAccDict;
            Console.WriteLine(accs[0].Balance);
            var stataccs = BankServices.clPassAccDictSt;
            foreach (var item in stataccs)
            {
                Console.WriteLine($"========= словарь {item.Key}");
                foreach (var ac in item.Value)
                {
                    Console.WriteLine($"{ac.AccNumber} {ac.Balance} {ac.CurrencyType.Sign}");
                }
            }
            /*var ret =  bankServ.PutMoneyAndChange(25, accs[0], "I-ПР012341");
             Console.WriteLine(ret.Balance);*/
        }
    }
}
