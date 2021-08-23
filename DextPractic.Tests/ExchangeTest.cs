using System;
using Xunit;
using BankSystem.Services;
using BankSystem.Models;

namespace DextPractic.Tests
{
    public class ExchangeTest
    {
        [Fact]
        public void GetExchange_10_USD_Eq_163_RUB()
        {
            //Arrange
            var testExchange = new Exchange();
            //Act 
            var result = testExchange.ConvertCurrency<Currency>(10, new USD(), new RUB());

            //Assert;
            Assert.Equal(163, result);

        }
    }
}
