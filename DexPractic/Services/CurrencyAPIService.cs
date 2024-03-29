﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using BankSystem.Models.Currencies;
using System.IO;
using System.Linq;

namespace BankSystem.Services
{
    public class CurrencyAPIService
    {
        // private readonly string _token;
        //Fixer.io по умолчанию предоставляет базовую валюту EUR
        //private readonly string fixerRequest = "http://data.fixer.io/api/latest?access_key=f452f9f69dd7109606654ab271268753&symbols=USD,EUR,MDL,RUB,UAH";
        private readonly string currencyLayerRequest = "http://api.currencylayer.com/live?access_key=3d54fc2d023d5c9318a60c177f233910&currencies=EUR,MDL,RUB,UAH";


        public async Task<CurrencyResponse> GetCurrencies()
        {
            HttpResponseMessage responseMessage;
            CurrencyResponse currencyResponse;
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles");
            using (var client = new HttpClient())
            {
                responseMessage = await client.GetAsync(currencyLayerRequest);
                responseMessage.EnsureSuccessStatusCode();

                string serializedMessage = await responseMessage.Content.ReadAsStringAsync();

                currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(serializedMessage);
                string reFormatCurrencyRepsonse = JsonConvert.SerializeObject(currencyResponse, Formatting.Indented);
                File.WriteAllText($"{path}\\CurrencyResponse.json", reFormatCurrencyRepsonse);
            }
            return currencyResponse;
        }

        public CurrencyResponse GetCurrencyResponseFromFile()
        {
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles", "CurrencyResponse.json");
            string currencyResponseJson = File.ReadAllText(path);

            CurrencyResponse currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(currencyResponseJson);
            return currencyResponse;
        }

        public decimal GetCurrencyRate(string sign)
        {
            var curResp = GetCurrencyResponseFromFile();
            var returnRate =
                (from currency in curResp.Quotes
                 where currency.Key == sign
                 select currency).FirstOrDefault();
            return returnRate.Value;
        }

        public static decimal GetStaticCurrencyRate(string sign)
        {
            string path = Path.Combine("G:", "C#Projects", "DexPractic_Bank_System", "BankSystemFiles", "CurrencyResponse.json");
            string currencyResponseJson = File.ReadAllText(path);

            CurrencyResponse currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(currencyResponseJson);
            var returnRate =
                (from currency in currencyResponse.Quotes
                 where currency.Key == sign
                 select currency).FirstOrDefault();
            return returnRate.Value;
        }

        public DateTime GetCurrencyResponseDate()
        {
            var myCurrencyDataFromFile = GetCurrencyResponseFromFile();
            var timeStamp = myCurrencyDataFromFile.Timestamp;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timeStamp);
            DateTime dateOfCurrencyData = dateTimeOffset.DateTime;

            return dateOfCurrencyData;

        }
    }
}
