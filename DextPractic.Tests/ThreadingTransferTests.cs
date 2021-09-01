using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BankSystem.Services;
using System.Threading;
using BankSystem.Models;

namespace DextPractic.Tests
{
    public class ThreadingTransferTests
    {
        private object locker = new object();
        [Fact]
        
        public void TwoThreadPutMoney_100_UAH_eq_200()
        {
            //Arrange
           // object locker = new object();
            var bankServ = new BankServices();

            var accs = bankServ.GetAccountsFromFile("I-ПР012341");

            BankServices.accsListSt = bankServ.GetAccountsFromFile("I-ПР012341");

            //Act 
            ThreadPool.QueueUserWorkItem(_ =>
            {
                //Thread.CurrentThread.Join();
                lock (locker)
                {
                    accs[1] = bankServ.PutMoney(100, accs[1]);
                }
                Thread.CurrentThread.Join();
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {

                lock (locker)
                {
                    accs[1] = bankServ.PutMoney(100, accs[1]);
                }
                
            });

            int result = (int)Math.Round(accs[0].Balance);

            //Assert

            Assert.Equal(2, result);
            //Assert.NotNull(accs);

        }

        [Fact]

        public void FindFromDict_eq_pair()
        {
            // Arrange

            var bankServ = new BankServices();

            //Act
            var transferCl = bankServ.FindFromDict("I-ПР012341");
            var ac = bankServ.GetAccountsFromFile("I-ПР012341");
            var myac = ac[0].Balance;

            //Assert
            Assert.Equal(731, myac);
        }

    }
}
