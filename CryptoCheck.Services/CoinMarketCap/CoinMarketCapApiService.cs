using AutoMapper;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoCheck.Services.CoinMarketCap
{
    public class CoinMarketCapApiService : ICryptoPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IApiBaseService _apiBaseService;
        private readonly ICacheService _cacheService;
        private readonly ILogger<ICryptoPriceService> _logger;
        private readonly string _baseUrl;
        private readonly string _auxFields;
        private readonly string _baseCurrencySymbol;

        public CoinMarketCapApiService(HttpClient httpClient, IMapper mapper, IConfiguration configuration, IApiBaseService apiBaseService, ICacheService cacheService, ILogger<ICryptoPriceService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _apiBaseService = apiBaseService ?? throw new ArgumentNullException(nameof(apiBaseService));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _baseUrl = configuration["coinMarketCapApi:baseUrl"];
            _auxFields = configuration["coinMarketCapApi:auxFields"];
            _baseCurrencySymbol = configuration["coinMarketCapApi:baseCurrencySymbol"];
        }

        public async Task<CryptoCurrencyPrice> GetCryptoPriceAsync(string symbol)
        {
            //construct request uri
            var uri = new Uri($"{_baseUrl.Trim('/')}/cryptocurrency/quotes/latest?symbol={symbol.ToLower()}&aux={_auxFields}&convert={_baseCurrencySymbol}");

            //get raw string response
            var quoteResponseString = await _apiBaseService.ExecuteRequest(_httpClient, uri);

            //extract root quote data
            var rootQuoteDataJsonPathSelector = $@"$.data.{symbol.ToUpper()}";
            var jToken = JToken.Parse(quoteResponseString);
            var rootQuoteData = JsonConvert.DeserializeObject<Models.CryptoCurrencyQuoteData>(jToken.SelectToken(rootQuoteDataJsonPathSelector)?.ToString());

            //extract currency specific data
            var currencySpecificQuoteJsonPathSelector = $@"{rootQuoteDataJsonPathSelector}.quote.{_baseCurrencySymbol.ToUpper()}";
            var currencySpecificQuote = JsonConvert.DeserializeObject<Models.CurrencyQuote>(jToken.SelectToken(currencySpecificQuoteJsonPathSelector)?.ToString());

            //assign current
            rootQuoteData.Quote.CurrencyQuote = currencySpecificQuote;

            var cryptoCurrencyPrice = _mapper.Map<CryptoCurrencyPrice>(rootQuoteData);

            //set base currency
            cryptoCurrencyPrice.CurrencySymbol = _baseCurrencySymbol.ToUpper();

            return cryptoCurrencyPrice;
        }

        public async Task<IList<CryptoCurrency>> GetAllCryptoCurrencies()
        {
            var allCryptoCurrencies = await _cacheService.GetItemAsync<List<CryptoCurrency>>("cryptocurrencies", getCryptoCurrencies, new CancellationToken());

            async Task<List<CryptoCurrency>> getCryptoCurrencies()
            {
                //construct request uri
                var uri = new Uri($"{_baseUrl.Trim('/')}/cryptocurrency/map");

                //get raw string response
                var quoteResponseString = await _apiBaseService.ExecuteRequest(_httpClient, uri);

                //extract 
                var cryptoCurrencysonPathSelector = $@"$.data";
                var jToken = JToken.Parse(quoteResponseString);
                var cryptoCurrencyMaps = JsonConvert.DeserializeObject<List<Models.CoinMarketCapCryptoCurrencyMap>>(jToken.SelectToken(cryptoCurrencysonPathSelector)?.ToString());

                var cryptoCurrencies = _mapper.Map<List<CryptoCurrency>>(cryptoCurrencyMaps);

                return cryptoCurrencies;
            }

            return allCryptoCurrencies;
        }
    }
}
