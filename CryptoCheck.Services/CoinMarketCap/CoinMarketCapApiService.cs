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
using System.Threading.Tasks;

namespace CryptoCheck.Services.CoinMarketCap
{
    public class CoinMarketCapApiService : ICryptoPriceService
    {
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IApiBaseService _apiBaseService;
        private readonly ILogger<ICryptoPriceService> _logger;
        private readonly string _baseUrl;
        private readonly string _auxFields;

        public CoinMarketCapApiService(HttpClient httpClient, IMapper mapper, IConfiguration configuration, IApiBaseService apiBaseService, ILogger<ICryptoPriceService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _apiBaseService = apiBaseService ?? throw new ArgumentNullException(nameof(apiBaseService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _ = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
            _baseUrl = configuration["coinMarketCapApi:baseUrl"];
            _auxFields = configuration["coinMarketCapApi:auxFields"];
        }

        public async Task<CryptoCurrencyPrice> GetCryptoPriceAsync(string symbol)
        {
            var uri = new Uri($"{_baseUrl.Trim('/')}/cryptocurrency/quotes/latest?symbol={symbol.ToLower()}&aux={_auxFields}");

            var quoteData = await _apiBaseService.ExecuteRequest<Models.CryptoCurrencyQuoteData>(_httpClient, uri, $@"$.data.{symbol.ToUpper()}");

            var cryptoCurrencyPrice = _mapper.Map<CryptoCurrencyPrice>(quoteData);

            return cryptoCurrencyPrice;
        }
    }
}
