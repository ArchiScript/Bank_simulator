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


        public static void Add<T>(T person) where T : Person
        {
            if (person is Client)
            {
                var client = person as Client;
                clients.Add(new Client
                {
                    Name = client.Name,
                    PassNumber = client.PassNumber,
                    DateOfBirth = client.DateOfBirth,
                    Id = client.Id
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



        public static Employee FindEmployee(string passNumber)
        {
            var emp = employees;
            //Client resultClient;

             return
            (from employee in emp
             where employee.PassNumber == passNumber
             select new Employee
             {
                 Name = employee.Name,
                 PassNumber = employee.PassNumber,
                 DateOfBirth = employee.DateOfBirth,
                 Id = employee.Id
             }).FirstOrDefault();
        }
        public static Client FindClient (string passNumber)
        {
            var cl = clients;
            //Client resultClient;

            return
            (from client in cl
             where client.PassNumber == passNumber
             select new Client
             {
                 Name = client.Name,
                 PassNumber = client.PassNumber,
                 DateOfBirth = client.DateOfBirth,
                 Id = client.Id
             }).FirstOrDefault();
        }
        public static IPerson Find<T>(T person) where T : IPerson
        {
           
            if (person is Employee)
            {
                 var pers = person as Employee;
            }
            
            


        }
    }

}

