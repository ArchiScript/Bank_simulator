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

            //BankServices.accsListSt = bankServ.GetAccountsFromFile("I-ПР012341");

            var accs = bankServ.GetAccountsFromPair(bankServ.FindFromFileDict("I-ПР012341"));
            //Act 
            ThreadPool.QueueUserWorkItem(_ =>
            {
                
                lock (locker)
                {
                    bankServ.PutMoneyAndChange(100, accs[1], "I-ПР012341");
                }
                //Thread.CurrentThread.Join();
            });

            ThreadPool.QueueUserWorkItem(_ =>
            {

                lock (locker)
                {
                    bankServ.PutMoneyAndChange(100, accs[1], "I-ПР012341");
                }
                //Thread.CurrentThread.Join();
            });

            int result = (int)Math.Round(accs[1].Balance);

            //Assert

            Assert.Equal(600, result);

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
