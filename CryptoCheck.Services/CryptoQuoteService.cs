﻿using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Services
{
    public class CryptoQuoteService : ICryptoQuoteService
    {
        private readonly ICryptoPriceService _cryptoPriceService;
        private readonly IExchangeRatesService _exchangeRateService;
        private readonly string _baseCurrencySymbol;
        private readonly string _conversionCurrencySymbols;

        public CryptoQuoteService(ICryptoPriceService cryptoPriceService, IExchangeRatesService exchangeRateService, IConfiguration configuration)
        {
            _cryptoPriceService = cryptoPriceService ?? throw new ArgumentNullException(nameof(cryptoPriceService));
            _exchangeRateService = exchangeRateService ?? throw new ArgumentNullException(nameof(exchangeRateService));

            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _baseCurrencySymbol = configuration["exchangeRatesApi:baseCurrencySymbol"];
            _conversionCurrencySymbols = configuration["exchangeRatesApi:conversionCurrencySymbols"];            
        }


        public async Task<CryptoQuote> GenerateQuoteAsync(CryptoQuoteRequest cryptoQuoteRequest)
        {
            var cryptoCurrencyPriceQuoteTask = _cryptoPriceService.GetCryptoPriceAsync(cryptoQuoteRequest.Symbol);

            var exchangeRatesTask = _exchangeRateService.GetExchangeRatesAsync(_baseCurrencySymbol, _conversionCurrencySymbols);

            await Task.WhenAll(cryptoCurrencyPriceQuoteTask, cryptoCurrencyPriceQuoteTask);

            return CreateQuote(cryptoCurrencyPriceQuoteTask.Result, exchangeRatesTask.Result);
        }

        //may want to map to domain models so you don't take a deep reliance on coin market cap

        private CryptoQuote CreateQuote(CryptoCurrencyPrice cryptoCurrencyPrice, ExchangeRates exchangeRates)
        {
            //get currency that crypto price is quoted in
            var cryptoCurrencyPriceSymbol = cryptoCurrencyPrice.CurrencySymbol;

            //get base currency used for exchange rate
            var exchangeBaseCurrencySymbol = exchangeRates.BaseSymbol;

            //determine the conversion ratio between them using the exchange rates
            var conversionRatioFromExchangeBaseToCryptoCurrencyBase = exchangeRates.Rates[exchangeBaseCurrencySymbol]/exchangeRates.Rates[cryptoCurrencyPriceSymbol];

            //convert the crypto price to the base exchange rate
            var cryptoPriceInExchangeRateBase = cryptoCurrencyPrice.Price * conversionRatioFromExchangeBaseToCryptoCurrencyBase;

            //loop and convert all currencies
            var currencyQuotes = new Dictionary<string, double>();

            foreach (var exchangeRate in exchangeRates.Rates)
            {
                currencyQuotes.Add(exchangeRate.Key, exchangeRate.Value * cryptoPriceInExchangeRateBase);
            }

            return new CryptoQuote
            {
                Name = cryptoCurrencyPrice.Name,
                Symbol = cryptoCurrencyPrice.CryptoSymbol,
                CurrencyQuotes = currencyQuotes,
                IsCachedResponse = false,
                IssuedAt = DateTime.UtcNow
            };
        }
    }
}
