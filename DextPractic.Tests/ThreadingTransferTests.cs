using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using BankSystem.Services;
using System.Threading;

namespace DextPractic.Tests
{
   /* public class ThreadingTransferTests
    {
        [Fact]

        public void TwoThreadTransfer_100_eq_200()
        {
            //Arrange
            object locker = new object();
            var bankServ = new BankServices();

            var exc = new Exchange();

            //Создаем переменную делегата и присваиваем ей адрес метода 
            var exchangeHandler = new BankServices.ExchangeDelegate(exc.ConvertCurrency);

            //Найти по номеру пасспорта и вернуть ключ-знач
            var transferCl = bankServ.FindFromDict("I-ПР012341");
            //Выбрать из ключ-значения список счетов
            var accs = bankServ.GetAccountsFromPair(transferCl);

            //Act 
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (locker)
                {
                    bankServ.MoneyTransfer(100, accs[0], accs[1],exchangeHandler);
                }
            });
            ThreadPool.QueueUserWorkItem(_ =>
            {
                lock (locker)
                {
                    bankServ.MoneyTransfer(100, accs[0], accs[1], exchangeHandler);
                }
            });


            //Assert


        }

    }*/
}
