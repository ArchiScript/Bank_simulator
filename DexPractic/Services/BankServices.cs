using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;
using System.Linq;
using BankSystem.Services;
using BankSystem.Exceptions;
using System.IO;
using Newtonsoft;
using Newtonsoft.Json;


namespace BankSystem.Services
{
    public class BankServices
    {
        public static List<Client> clients = new List<Client>();
        public static List<Employee> employees = new List<Employee>();
        public static Dictionary<Client, List<Account>> clientsDict = new Dictionary<Client, List<Account>>();
        public static Dictionary<string, List<Account>> clPassAccDict = new Dictionary<string, List<Account>>();


        //Определяем делегат
        public delegate decimal ExchangeDelegate(decimal sum, Currency convertFrom, Currency convertTo);


        // Объявляем обобщенный делегат func
        //public Func<decimal, Currency, Currency, decimal> funcExc = (sum, cur, cur2) => sum / cur.Rate * cur2.Rate;
        public Func<decimal, Currency, Currency, decimal> funcExc;



        //ДОБАВЛЯЕТ В ЛИСТ ПЕРСОНУ ЕСЛИ КЛИЕНТ
        private void IfEmployeeAdd<T>(T person) where T : Person
        {
            var employee = person as Employee;
            try
            {
                bool ageAllowed = IsAgeAllowed(18, DateTime.Parse(employee.DateOfBirth));
                if (!ageAllowed)
                {
                    throw new BankAdultException("Вы не достигли совершеннолетия");
                }
                if (!employees.Contains(employee))
                {
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
                else
                {
                    Console.WriteLine($"Сотрудник {employee} уже присутствует в базе");
                }
            }
            catch (BankAdultException e)
            {
                Console.WriteLine($"Возникла ошибка доступа по возрасту {e}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Перехвачено исключение {e.Message}");
            }
        }


        //ДОБАВЛЯЕТ В ЛИСТ ПЕРСОНУ ЕСЛИ СОТРУДНИК
        private void IfClientAdd<T>(T person) where T : Person
        {
            var client = person as Client;
            try
            {
                bool ageAllowed = IsAgeAllowed(18, DateTime.Parse(client.DateOfBirth));
                if (!ageAllowed)
                {
                    throw new BankAdultException("Вы не достигли совершеннолетия");
                }
                if (!(clients.Count() == 0) && !clients.Contains(client))
                {
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
                    Console.WriteLine($"Клиент {client} уже присутствует в базе");
                }
            }
            catch (BankAdultException e)
            {
                Console.WriteLine($"Возникла ошибка доступа по возрасту {e}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Перехвачено исключение {e.Message}");
            }
        }


        //ДОБАВЛЯЕТ В ЛИСТ ПЕРСОНУ, ПРОВЕРЯЕТ НА ВОЗРАСТ, ------ПИШЕТ В ФАЙЛ (JSON)
        public void Add<T>(T person) where T : Person
        {
            //D:\WEBDEV\Dex_Practic
            //G:\C#Projects\DexPractic_Bank_System
            //string path = Path.Combine("D:", "WEBDEV", "Dex_Practic", "BankSystemFiles");
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            if (person is Client)
            {
                IfClientAdd(person);
            }
            else
            {
                IfEmployeeAdd(person);
            }
            //var pathTxtFile = $"{path}\\Clients&Employees.txt";
            var pathJsonCl = $"{path}\\Clients.json";
            var pathJsonEmp = $"{path}\\Employees.json";

            string jsonClList = JsonConvert.SerializeObject(clients, Formatting.Indented);
            string jsonEmpList = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText(pathJsonCl, jsonClList);
            File.WriteAllText(pathJsonEmp, jsonEmpList);
        }


        //ДОБАВЛЯЕТ В СЛОВАРЬ НОВЫЙ СЧЕТ КЛИЕНТУ, ИЛИ НОВОГО КЛИЕНТА И СЧЕТ, ------ПИШЕТ В ФАЙЛ (JSON)
        public void AddClientAccount(Client client, Account account)
        {
            
            if (!(clientsDict.Count == 0) && clientsDict.Keys.Contains(client))
            {
                Console.WriteLine($"Клиент {client.Name} уже есть в базе...добавляем счет");
                var clDict = clientsDict[client];
                clDict.Add(account);
            }
            else
            {
                Console.WriteLine($"Добавляется клиент и счет");
                clientsDict.Add(client, new List<Account>());
                var accNew = clientsDict[client];
                accNew.Add(account);
                foreach (var item in accNew)
                {
                    Console.WriteLine(item.AccNumber);
                }
            }
            WriteClientAccountToFile();

        }


        // ПИШЕТ СЧЕТА И КЛИЕНТОВ В ФАЙЛ (JSON)
        public void WriteClientAccountToFile()
        {
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            List<Account> accsAllList = new List<Account>();
            List<Client> clList = new List<Client>();
            Dictionary<string, List<Account>> clPassAccDic = new Dictionary<string, List<Account>>();
            foreach (var pair in clientsDict)
            {
                clList.Add(pair.Key);
                List<Account> everyPairAccList = new List<Account>();
                foreach (var acc in pair.Value)
                {
                    everyPairAccList.Add(new Account
                    {
                        AccNumber = acc.AccNumber,
                        Balance = acc.Balance,
                        CurrencyType = acc.CurrencyType
                    }
);
                    accsAllList.Add(acc);
                }
                clPassAccDic.Add(pair.Key.PassNumber, everyPairAccList);
            }

            string pathToClAccJson = Path.Combine(path, "ClientsPass&AccountsDict.json");
            string pathToClJson = Path.Combine(path, "Clients.json");
            string pathToAccJson = Path.Combine(path, "Accounts.json");

            string accListJson = JsonConvert.SerializeObject(accsAllList, Formatting.Indented);
            string clListJson = JsonConvert.SerializeObject(clList, Formatting.Indented);
            string clDictJson = JsonConvert.SerializeObject(clPassAccDic, Formatting.Indented);

            File.WriteAllText(pathToClAccJson, clDictJson);
            File.WriteAllText(pathToAccJson, accListJson);
            File.WriteAllText(pathToClJson, clListJson);
        }



        //ВОЗВРАЩАЕТ КЛЮЧ-ЗНАЧЕНИЕ С ПЕРЕДАЧЕЙ ПАРАМЕТРОМ НОМЕРА ПАССПОРТА --------- ИСТОЧНИК ДАННЫХ ИЗ ФАЙЛА (JSON)
        public Dictionary<Client, List<Account>> FindFromDict(string passNumber)
        {
            Dictionary<Client, List<Account>> newDic = new Dictionary<Client, List<Account>>();
            if (!(GetDictFromFile().Count == 0))
            {
                var findNameCl =
                     from client in clientsDict
                     where client.Key.PassNumber == passNumber
                     select client;
                foreach (var pair in findNameCl)
                {
                    newDic.Add(pair.Key, pair.Value);
                }
            }

            return newDic;
        }


        //ВОЗВРАЩАЕТ ЛИСТ КЛИЕНТОВ, СЧИТАННЫЙ -------- ИЗ ФАЙЛА (JSON)
        private List<Client> ClientFromFileToList()
        {
            //List<Client> fromFileClList = new List<Client>();
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            string pathToCl = Path.Combine(path, "Clients.txt");
            string pathToClJson = Path.Combine(path, "Clients.json");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            string jsonClList = File.ReadAllText(pathToClJson);
            List<Client> fromFileClList = JsonConvert.DeserializeObject<List<Client>>(jsonClList);
            return fromFileClList;
        }


        //ВОЗВРАЩАЕТ СЛОВАРЬ С КЛЮЧОМ ВВИДЕ номеров пасспорта -------- ИЗ ФАЙЛА (JSON)
        private Dictionary<string, List<Account>> AccountFromFileToDict()
        {
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            string pathToAcc = Path.Combine(path, "Accounts.txt");
            string pathToAccJson = Path.Combine(path, "Accounts.json");
            string pathToAccClDict = Path.Combine(path, "ClientsPass&AccountsDict.json");
            if (!File.Exists(pathToAcc))
            {
                File.Create(pathToAcc);
            }

            string jsonAccClDict = File.ReadAllText(pathToAccClDict);
            var fromFileAccClDict = JsonConvert.DeserializeObject<Dictionary<string, List<Account>>>(jsonAccClDict);

            return fromFileAccClDict;
        }


        //ВОЗВРАЩАЕТ СЛОВАРЬ С ДАННЫМИ ---------------ИЗ ФАЙЛА (JSON)
        public Dictionary<Client, List<Account>> GetDictFromFile()
        {
            Dictionary<Client, List<Account>> fromFileClListAccDict = new Dictionary<Client, List<Account>>();
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            string pathToClAcc = Path.Combine(path, "Clients&Accounts.txt");
            string pathToClAccJson = Path.Combine(path, "Clients&Accounts.json");
            if (!File.Exists(pathToClAcc))
            {
                File.Create(pathToClAcc);
            }
            var fromFileClList = ClientFromFileToList();
            var fromFileAccClDict = AccountFromFileToDict();

            foreach (var pair in fromFileAccClDict)
            {
                foreach (var cl in fromFileClList)
                {
                    if (pair.Key == cl.PassNumber)
                    {
                        fromFileClListAccDict.Add(cl, pair.Value);
                    }
                }
            }
            return fromFileClListAccDict;
        }

        //ВОЗВРАЩАЕТ IPerson ИЗ ЛИСТА С ПЕРЕДАЧЕЙ ПАРАМЕТРОМ НОМЕРА ПАССПОРТА
        public IPerson Find<T>(string passNumber) where T : IPerson
        {
            IPerson person = null;
            if (employees.Count != 0 && clients.Count != 0)
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
            }else { return person; }

        }


        //ВОЗВРАЩАЕТ IPerson ИЗ ЛИСТА С ПЕРЕДАЧЕЙ ПАРАМЕТРОМ ОБЪЕКТА
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


        // ИСПОЛЬЗУЯ FUNC ПЕРЕВОДИТ СРЕДСТВА С ОДНОГО СЧЕТА КЛИЕНТА НА ДРУГОЙ С КОНВЕРТАЦИЕЙ
        //подставляем в качестве параметра func вместе с сигнатурой
        public void MoneyTransferFunc(decimal sum, Account accountFrom, Account accountTo, Func<decimal, Currency, Currency, decimal> funcExc)
        {
            try
            {
                if (accountFrom.Balance < sum)
                {
                    throw new LowBalanceException($"Недостаточно средств на счете {accountFrom.AccNumber} " +
                        $"для осуществления перевода суммой {sum} {accountFrom.CurrencyType.Sign}");
                }
                if (funcExc != null)
                {
                    decimal result = funcExc(sum, accountFrom.CurrencyType, accountTo.CurrencyType); ;

                    accountFrom.Balance -= sum;
                    accountTo.Balance += result;
                    Console.WriteLine($"Со счета {accountFrom.AccNumber} списано {sum} {accountFrom.CurrencyType.Sign}" +
                        $" на счет {accountTo.AccNumber} в валюте {accountTo.CurrencyType.Sign} пришло {result} {accountTo.CurrencyType.Sign}\n на Вашем счете осталось " +
                        $"{accountFrom.Balance} {accountFrom.CurrencyType.Sign} " +
                        $"\n на счете {accountTo.AccNumber} осталось {accountTo.Balance} {accountTo.CurrencyType.Sign}");
                }
            }
            catch (LowBalanceException e)
            {
                Console.WriteLine($"Возникла ошибка при проведении банковской операции {e}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Перехвачено исключение {e.Message}");
            }
        }

        //ВОЗВРАЩАЕТ СПИСОК СЧЕТОВ ЗАРАНЕЕ НАЙДЕННОГО КЛИЕНТА КЛИЕНТА
        public List<Account> GetAccountsFromPair(Dictionary<Client, List<Account>> keyValuePair)
        {
            return keyValuePair.FirstOrDefault().Value;
        }


        //ВОЗВРАЩАЕТ КЛИЕНТА ПО НОМЕРУ ПАССПОРТА ИЗ СЛОВАРЯ---------ИСТОЧНИК ДАННЫХ ИЗ ФАЙЛА
        public Client GetClientFromDict(string passNumber)
        {
            try
            {
                return
                   (from client in clientsDict
                    where client.Key.PassNumber == passNumber
                    select client).FirstOrDefault().Key;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }

        }

        //ПРОВЕРЯЕТ НА МИНИМАЛЬНЫЙ ДОПУСТИМЫЙ ВОЗРАСТ
        bool IsAgeAllowed(int minAge, DateTime birthDate)
        {
            double age = Math.Round(DateTime.Now.Subtract(birthDate).TotalDays / 365.25, 2);
            return (age > minAge);
        }



        public void ShowClients()
        {
            foreach (var cl in clients)
            {
                Console.WriteLine($"{cl.Name} {cl.PassNumber} {cl.Id} {cl.DateOfBirth}");
            }
        }

    }



}

