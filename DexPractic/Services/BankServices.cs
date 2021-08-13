using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;
using System.Linq;
using BankSystem.Services;
using BankSystem.Exceptions;
using System.IO;


namespace BankSystem.Services
{
    public class BankServices
    {
        public static List<Client> clients = new List<Client>();
        public static List<Employee> employees = new List<Employee>();
        public static Dictionary<Client, List<Account>> clientsDict = new Dictionary<Client, List<Account>>();
        private string listClientData;
        private string listEmployeeData;
        private string dictClientData;
        //Определяем делегат
        public delegate decimal ExchangeDelegate(decimal sum, Currency convertFrom, Currency convertTo);


        // Объявляем обобщенный делегат func
        //public Func<decimal, Currency, Currency, decimal> funcExc = (sum, cur, cur2) => sum / cur.Rate * cur2.Rate;
        public Func<decimal, Currency, Currency, decimal> funcExc;


        //ДОБАВЛЯЕТ В ЛИСТ ПЕРСОНУ, ПРОВЕРЯЕТ НА ВОЗРАСТ, ------ПИШЕТ В ФАЙЛ
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
                var client = person as Client;
                try
                {
                    bool ageAllowed = IsAgeAllowed(18, DateTime.Parse(client.DateOfBirth));
                    if (!ageAllowed)
                    {
                        throw new BankAdultException("Вы не достигли совершеннолетия");
                    }
                    clients.Add(new Client
                    {
                        Name = client.Name,
                        PassNumber = client.PassNumber,
                        DateOfBirth = client.DateOfBirth,
                        Id = client.Id,
                    });

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
            else
            {
                var employee = person as Employee;
                try
                {
                    bool ageAllowed = IsAgeAllowed(18, DateTime.Parse(employee.DateOfBirth));
                    if (!ageAllowed)
                    {
                        throw new BankAdultException("Вы не достигли совершеннолетия");
                    }
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
                catch (BankAdultException e)
                {
                    Console.WriteLine($"Возникла ошибка доступа по возрасту {e}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Перехвачено исключение {e.Message}");
                }
            }
            listClientData = "";
            listEmployeeData = "";
            foreach (var client in clients)
            {
                using (FileStream fileStream = new FileStream($"{path}\\Clients&Employees.txt", FileMode.Truncate))
                {
                    listClientData += $"Клиент,{client.Name},{client.PassNumber},{client.DateOfBirth},{Convert.ToString(client.Id)}{Environment.NewLine}";
                    byte[] array = System.Text.Encoding.Default.GetBytes(listClientData);
                    fileStream.Write(array, 0, array.Length);
                }
            }

            foreach (var emp in employees)
            {
                using (FileStream fileStream1 = new FileStream($"{path}\\Clients&Employees.txt", FileMode.Append))
                {
                    listEmployeeData += $"Сотрудник,{emp.Name},{emp.PassNumber}," +
                        $"{emp.DateOfBirth},{emp.DateOfEmployment}," +
                        $"{emp.Position},{Convert.ToString(emp.Id)}{Environment.NewLine}";
                    byte[] array = System.Text.Encoding.Default.GetBytes(listEmployeeData);
                    fileStream1.Write(array, 0, array.Length);
                }
            }

        }


        //ДОБАВЛЯЕТ В СЛОВАРЬ НОВЫЙ СЧЕТ КЛИЕНТУ, ИЛИ НОВОГО КЛИЕНТА И СЧЕТ, ------ПИШЕТ В ФАЙЛ
        public void AddClientAccount(Client client, Account account)
        {
            if (clientsDict.Keys.Contains(client))
            {
                Console.WriteLine($"У клиента {client.Name} уже есть счет");
                var accList = clientsDict[client];
                accList.Add(account);
            }
            else
            {
                Console.WriteLine($"Добавляется счет и клиент");
                clientsDict.Add(client, new List<Account>());
                var accNew = clientsDict[client];
                accNew.Add(new Account
                {
                    AccNumber = account.AccNumber,
                    Balance = account.Balance,
                    CurrencyType = account.CurrencyType
                });
            }

            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            dictClientData = "";
            foreach (var pair in clientsDict)
            {
                foreach (var acc in pair.Value)
                {
                    dictClientData += $"{pair.Key.Name},{pair.Key.PassNumber},{pair.Key.DateOfBirth}," +
                    $"{pair.Key.Id},{acc.AccNumber},{acc.Balance},{acc.CurrencyType.Sign}{Environment.NewLine}";
                }
            }
            string pathToFile = $@"{path}\Clients&Accounts.txt";
            FileStream fstrm;
            if (File.Exists(pathToFile))
            {
                fstrm = new FileStream(pathToFile, FileMode.Truncate);
            }
            else
            {
                fstrm = new FileStream(pathToFile, FileMode.Append);
            }
            using (FileStream fileStream = fstrm)
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(dictClientData);
                fileStream.Write(array, 0, array.Length);
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


        //ВОЗВРАЩАЕТ СЛОВАРЬ С ДАННЫМИ ИЗ ФАЙЛА
        public Dictionary<Client, List<Account>> GetDictFromFile()
        {
            Dictionary<Client, List<Account>> returnDict = new Dictionary<Client, List<Account>>();
            string path = $@"G:\C#Projects\DexPractic_Bank_System\BankSystemFiles";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);

            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            string pathToFile = $@"{path}\Clients&Accounts.txt";
            if (!File.Exists(pathToFile))
            {
                File.Create(pathToFile);
            }
            using (FileStream fileStream = new FileStream(pathToFile, FileMode.Open))
            {
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                string readData = System.Text.Encoding.Default.GetString(array);
                var pairsArr = readData.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
                foreach (var pair in pairsArr)
                {
                    var property = pair.Split(",", StringSplitOptions.RemoveEmptyEntries);

                    Currency cur;
                    switch (property[6])
                    {
                        case "USD":
                            cur = new USD();
                            break;
                        case "UAH":
                            cur = new UAH();
                            break;
                        case "EUR":
                            cur = new EUR();
                            break;
                        case "MDL":
                            cur = new MDL();
                            break;
                        default:
                            cur = new USD();
                            break;
                    }
                    //Console.WriteLine(cur.Rate);
                    List<Account> accList = new List<Account>();
                    accList.Add(new Account
                    {
                        AccNumber = (ulong)Convert.ToInt64(property[4]),
                        Balance = Convert.ToDecimal(property[5]),
                        CurrencyType = cur
                    });
                    returnDict.Add(new Client
                    {
                        Name = property[0],
                        PassNumber = property[1],
                        DateOfBirth = property[2],
                        Id = Convert.ToInt32(property[3])

                    }, accList);
                }
            }
            return returnDict;
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


        //ВОЗВРАЩАЕТ КЛИЕНТА ПО НОМЕРУ ПАССПОРТА ИЗ СЛОВАРЯ
        public Client GetClientFromDict(string passNumber)
        {
            var findNameCl =
                     from client in clientsDict
                     where client.Key.PassNumber == passNumber
                     select client;

            return findNameCl.FirstOrDefault().Key;
        }

        //ПРОВЕРЯЕТ НА МИНИМАЛЬНЫЙ ДОПУСТИМЫЙ ВОЗРАСТ
        bool IsAgeAllowed(int minAge, DateTime birthDate)
        {
            double age = Math.Round(DateTime.Now.Subtract(birthDate).TotalDays / 365.25, 2);
            return (age > minAge);
        }

    }



}

