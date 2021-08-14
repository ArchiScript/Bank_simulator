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
        public static Dictionary<List<Account>, Client> accountsDict = new Dictionary<List<Account>, Client>();
        private string listClientData;
        private string listEmployeeData;
        private string dictClientData;
        private string dictAccountData;
        private string dictAllData;
        public static List<Client> testClList = new List<Client>();
        public static Dictionary<Account, string> testAccDict = new Dictionary<Account, string>();
        public static Dictionary<Account, Client> testAccClDict = new Dictionary<Account, Client>();
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
                Console.WriteLine($"Клиент {client.Name} уже есть в базе...добавляем счет");
                var clDict = clientsDict[client];
                clDict.Add(account);
                //Добавляем в словарь со счетами в качестве ключа
                List<Account> acList = new List<Account>();
                acList.Add(account);
                //accountsDict.Add(acList, client);
            }
            else
            {
                Console.WriteLine($"Добавляется клиент и счет");
                clientsDict.Add(client, new List<Account>());
                var accNew = clientsDict[client];
                accNew.Add(account);
                List<Account> acList = new List<Account>();
                acList.Add(account);
                //accountsDict.Add(acList, client);

            }

            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            dictClientData = "";
            dictAccountData = "";
            dictAllData = "";
            var br = Environment.NewLine;
            foreach (var pair in clientsDict)
            {
                dictAllData += $"{pair.Key.Name};{pair.Key.PassNumber};{pair.Key.DateOfBirth};{pair.Key.Id}/";
                dictClientData += $"{pair.Key.Name},{pair.Key.PassNumber},{pair.Key.DateOfBirth},{pair.Key.Id}{br}";
                foreach (var acc in pair.Value)
                {
                    dictAllData += $"{acc.AccNumber},{acc.Balance},{acc.CurrencyType.Sign};";

                    dictAccountData += $"{acc.AccNumber},{acc.Balance},{acc.CurrencyType.Sign},{pair.Key.PassNumber}{br}";
                }
                dictAllData += $"{br}";
            }
            string pathToClAcc = $@"{path}\Clients&Accounts.txt";
            string pathToClient = $@"{path}\Clients.txt";
            string pathToAccount = $@"{path}\Accounts.txt";
            FileStream fstrmCl;
            FileStream fstrmAc;
            FileStream fstrmClAc;
            if (File.Exists(pathToClient) && File.Exists(pathToAccount) && File.Exists(pathToClAcc))
            {
                fstrmCl = new FileStream(pathToClient, FileMode.Truncate);
                fstrmAc = new FileStream(pathToAccount, FileMode.Truncate);
                fstrmClAc = new FileStream(pathToClAcc, FileMode.Truncate);
            }
            else
            {
                fstrmCl = new FileStream(pathToClient, FileMode.Append);
                fstrmAc = new FileStream(pathToAccount, FileMode.Append);
                fstrmClAc = new FileStream(pathToClAcc, FileMode.Append);
            }
            using (FileStream fileStream = fstrmCl)
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(dictClientData);
                fileStream.Write(array, 0, array.Length);
            }
            using (FileStream fileStream = fstrmAc)
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(dictAccountData);
                fileStream.Write(array, 0, array.Length);
            }
            using (FileStream fileStream = fstrmClAc)
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(dictAllData);
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
            Dictionary<List<Account>, Client> returnDictReverse = new Dictionary<List<Account>, Client>();
            string path = $@"G:\C#Projects\DexPractic_Bank_System\BankSystemFiles";
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            var br = Environment.NewLine;
            var trimSplit = StringSplitOptions.RemoveEmptyEntries;
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            string pathToClAcc = $@"{path}\Clients&Accounts.txt";
            string pathToCl = $@"{path}\Clients.txt";
            string pathToAcc = $@"{path}\Accounts.txt";
            if (!File.Exists(pathToClAcc))
            {
                File.Create(pathToClAcc);
            }

            //////////////Чтение файла и добавление КЛИЕНТА в лист
            using (FileStream fileStream = new FileStream(pathToCl, FileMode.Open))
            {
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                string readData = System.Text.Encoding.Default.GetString(array);
                var ClArr = readData.Split(br, StringSplitOptions.RemoveEmptyEntries);
                foreach (var cl in ClArr)
                {
                    var properties = cl.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    testClList.Add(new Client
                    {
                        Name = properties[0],
                        PassNumber = properties[1],
                        DateOfBirth = properties[2],
                        Id = Convert.ToInt32(properties[3])
                    });
                }
            }

            //////////////Чтение файла и добавление  СЧЕТА в лист
            using (FileStream fileStream = new FileStream(pathToAcc, FileMode.Open))
            {
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                string readData = System.Text.Encoding.Default.GetString(array);
                var AccArr = readData.Split(br, StringSplitOptions.RemoveEmptyEntries);

                foreach (var acc in AccArr)
                {
                    var props = acc.Split(",", StringSplitOptions.RemoveEmptyEntries);
                    Currency cur;
                    switch (props[2])
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
                        case "RUB":
                            cur = new RUB();
                            break;
                        default:
                            cur = new USD();
                            break;
                    }
                    testAccDict.Add(new Account
                    {
                        AccNumber = (ulong)Convert.ToUInt64(props[0]),
                        Balance = Convert.ToDecimal(props[1]),
                        CurrencyType = cur
                    }, props[3]);
                }
            }
            ////////////////////Совмещение аккаунтов и клиентов
            foreach (var pair in testAccDict)
            {
                Console.WriteLine($"{pair.Key.AccNumber} {pair.Key.Balance} {pair.Key.CurrencyType.Sign} {pair.Value}");
            }

            ///////////////Чтение файла  и добавление акк и кл в словарь

            using (FileStream fileStream = new FileStream(pathToClAcc, FileMode.Open))
            {
                byte[] array = new byte[fileStream.Length];
                fileStream.Read(array, 0, array.Length);
                string readData = System.Text.Encoding.Default.GetString(array);
                var pairsArr = readData.Split(br, trimSplit);
                var currentAccList = new List<Account>();
                foreach (var pair in pairsArr)
                {
                    var objs = pair.Split("/", trimSplit);
                    var clProps = objs[0].Split(";", trimSplit);

                    ///Добавляем в клиента данные из файла
                    var currentCl = new Client
                    {
                        Name = clProps[0],
                        DateOfBirth = clProps[1],
                        PassNumber = clProps[2],
                        Id = Convert.ToInt32(clProps[3])
                    };

                    var accs = objs[1].Split(";", trimSplit);
                    
                    foreach (var acc in accs)
                    {

                        var accProps = acc.Split(",", trimSplit);

                        Console.WriteLine($"-------{acc}  {accProps[0]} {accProps[1]} {accProps[2]}");
                        Currency cur;
                        switch (accProps[2])
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
                            case "RUB":
                                cur = new RUB();
                                break;
                            default:
                                cur = new USD();
                                break;
                        }

                        
                        currentAccList.Add(new Account
                        {
                            AccNumber = (ulong)Convert.ToInt64(accProps[0]),
                            Balance = Convert.ToDecimal(accProps[1]),
                            CurrencyType = cur
                        });

                       
                    }
                    returnDict.Add(currentCl, currentAccList);
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

