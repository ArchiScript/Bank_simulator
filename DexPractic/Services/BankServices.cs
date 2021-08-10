using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;
using System.Linq;
using BankSystem.Services;

namespace BankSystem.Services
{
    public class BankServices
    {
        public static List<Client> clients = new List<Client>();
        public static List<Employee> employees = new List<Employee>();
        public static Dictionary<Client, List<Account>> clientsDict = new Dictionary<Client, List<Account>>();

        //Определяем делегат
        public delegate decimal ExchangeDelegate(decimal sum, Currency convertFrom, Currency convertTo);
        
        //Обобщенный делегат Func
        public Func<decimal, Currency, Currency, decimal> ExchangeFunc;
        //ДОБАВЛЯЕТ В ЛИСТ ПЕРСОНУ
        public void Add<T>(T person) where T : Person
        {
            if (person is Client)
            {
                var client = person as Client;
                clients.Add(new Client
                {
                    Name = client.Name,
                    PassNumber = client.PassNumber,
                    DateOfBirth = client.DateOfBirth,
                    Id = client.Id,

                });
            }
            else
            {
                var employee = person as Employee;
                employees.Add(new Employee
                {
                    Name = employee.Name,
                    PassNumber = employee.PassNumber,
                    DateOfBirth = employee.DateOfBirth,
                    DateOfEmployment = employee.DateOfEmployment,
                    Position = employee.Position,
                    Id = employee.Id
                });
            }
        }


        //ДОБАВЛЯЕТ НОВЫЙ СЧЕТ КЛИЕНТУ, ИЛИ НОВОГО КЛИЕНТА И СЧЕТ
        public void AddClientAccount(Client client, Account account)
        {
            if (clientsDict.Keys.Contains(client))
            {
                var accList = clientsDict[client];
                accList.Add(account);
            }
            else
            {
                clientsDict.Add(client, new List<Account>());
                var accNew = clientsDict[client];
                accNew.Add(new Account
                {
                    AccNumber = account.AccNumber,
                    Balance = account.Balance,
                    CurrencyType = account.CurrencyType
                });
            }
        }


        //ВОЗВРАЩАЕТ КЛЮЧ-ЗНАЧЕНИЕ С ПЕРЕДАЧЕЙ ПАРАМЕТРОМ НОМЕРА ПАССПОРТА
        public Dictionary<Client, List<Account>> FindFromDict(string passNumber)
        {
            var findNameCl =
                     from client in clientsDict
                     where client.Key.PassNumber == passNumber
                     select client;

            Dictionary<Client, List<Account>> newDic = new Dictionary<Client, List<Account>>();
            foreach (var pair in findNameCl)
            {
                newDic.Add(pair.Key, pair.Value);
            }
            return newDic;
        }


        //ВОЗВРАЩАЕТ IPerson С ПЕРЕДАЧЕЙ ПАРАМЕТРОМ НОМЕРА ПАССПОРТА
        public IPerson Find<T>(string passNumber) where T : IPerson
        {
            var findNameEmp =
           from employee in employees
           where employee.PassNumber == passNumber
           select employee;
            if (findNameEmp.Count() == 0)
            {
                var findNameCl =
                    from client in clients
                    where client.PassNumber == passNumber
                    select client;
                return findNameCl.FirstOrDefault();
            }
            else
            {
                return findNameEmp.FirstOrDefault();
            }
        }


        //ВОЗВРАЩАЕТ IPerson С ПЕРЕДАЧЕЙ ПАРАМЕТРОМ ОБЪЕКТА
        public IPerson Find<T>(T person) where T : IPerson
        {

            if (person is Employee)
            {
                var findEmp =
                    from employee in employees
                    where employee.PassNumber == person.PassNumber
                    select employee;
                return findEmp.FirstOrDefault();
            }
            else
            {
                var findCl =
                  from client in clients
                  where client.PassNumber == person.PassNumber
                  select client;
                return findCl.FirstOrDefault();
            }
        }


        //ПЕРЕВОДИТ СРЕДСТВА С ОДНОГО СЧЕТА КЛИЕНТА НА ДРУГОЙ С КОНВЕРТАЦИЕЙ
        //Передаем в параметры метода экземпляр делегата 
        public void MoneyTransfer(decimal sum, Account accountFrom, Account accountTo, ExchangeDelegate exchangeDelegate)
        {
            if (accountFrom.Balance < sum)
            {
                Console.WriteLine($"Недостаточно средств на счете {accountFrom}");
            }
            else
            {

                if (exchangeDelegate != null)
                {

                    // Вызываем делегат путем передачи параметров (тоже что exchangeDelegate.Invoke( , , )
                    //и присваиваем переменной результат метода, подписанного на этот делегат, то есть ConvertCurrency
                    decimal result = exchangeDelegate(sum, accountFrom.CurrencyType, accountTo.CurrencyType);

                    accountFrom.Balance -= sum;
                    accountTo.Balance += result;
                    Console.WriteLine($"Со счета {accountFrom.AccNumber} списано {sum} {accountFrom.CurrencyType.Sign}" +
                        $" на счет {accountTo.AccNumber} в валюте {accountTo.CurrencyType.Sign} пришло {result} {accountTo.CurrencyType.Sign}\n на Вашем счете осталось " +
                        $"{accountFrom.Balance} {accountFrom.CurrencyType.Sign} " +
                        $"\n на счете {accountTo.AccNumber} осталось {accountTo.Balance} {accountTo.CurrencyType.Sign}");

                }

            }
        }

        //ВОЗВРАЩАЕТ СПИСОК СЧЕТОВ ЗАРАНЕЕ НАЙДЕННОГО КЛИЕНТА КЛИЕНТА
        public List<Account> GetAccountsFromPair(Dictionary<Client, List<Account>> keyValuePair)
        {
            return keyValuePair.FirstOrDefault().Value;
        }


        //ВОЗВРАЩАЕТ КЛИЕНТА ПО НОМЕРУ ПАССПОРТА ИЗ СЛОВАРЯ
        public Client GetClientFromDict(string passNumber)
        {
            var findNameCl =
                     from client in clientsDict
                     where client.Key.PassNumber == passNumber
                     select client;

            return findNameCl.FirstOrDefault().Key;
        }


    }



}

