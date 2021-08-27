using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using BankSystem.Models.Currencies;

namespace BankSystem.Services
{
    public class CurrencyAPIService
    {
        // private readonly string _token;
        //Fixer.io по умолчанию предоставляет базовую валюту EUR
        //private readonly string fixerRequest = "http://data.fixer.io/api/latest?access_key=f452f9f69dd7109606654ab271268753&symbols=USD,EUR,MDL,RUB,UAH";
        private readonly string currencyLayerRequest = "http://api.currencylayer.com/live?access_key=3d54fc2d023d5c9318a60c177f233910&currencies=EUR,MDL,RUB,UAH";
        CurrencyResponse currencyResponse;
        public async  Task<CurrencyResponse> GetCurrencies()
        {
            HttpResponseMessage responseMessage;
            CurrencyResponse currencyResponse;
            using (var client = new HttpClient())
            {
                responseMessage = await client.GetAsync(currencyLayerRequest);
                responseMessage.EnsureSuccessStatusCode();

                string serializedMessage = await responseMessage.Content.ReadAsStringAsync();

                //Console.WriteLine(serializedMessage);
                currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(serializedMessage);
                //Console.WriteLine(currencyResponse);
                
            }
            return currencyResponse;
        }
    }
}
