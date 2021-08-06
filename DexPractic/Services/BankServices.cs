using System;
using System.Collections.Generic;
using System.Text;
using BankSystem.Models;

namespace BankSystem.Services
{
    public class BankServices
    {
        public List<Client> clients = new List<Client>();
        public List<Employee> employees = new List<Employee>();


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
    }

}

