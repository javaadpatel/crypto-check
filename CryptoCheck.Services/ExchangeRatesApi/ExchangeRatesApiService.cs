using AutoMapper;
using CryptoCheck.Core.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.Services.ExchangeRatesApi
{
    public class ExchangeRatesApiService : IExchangeRatesService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiBaseService _apiBaseService;
        private readonly IMapper _mapper;
        private readonly ILogger<IExchangeRatesService> _logger;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public ExchangeRatesApiService(HttpClient httpClient, IApiBaseService apiBaseService, IMapper mapper, IConfiguration configuration, ILogger<IExchangeRatesService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseService = apiBaseService ?? throw new ArgumentNullException(nameof(apiBaseService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _baseUrl = configuration["exchangeRatesApi:baseUrl"];
            _apiKey = configuration["exchangeRatesApi:apiKey"];
        }

        public async Task<Core.Models.ExchangeRates> GetExchangeRatesAsync(string baseCurrencySymbol, string conversionCurrencySymbols)
        {
            //testing
            //            JObject o = JObject.Parse(@"{
            //    'success': true,
            //    'timestamp': 1622122024,
            //    'base': 'EUR',
            //    'date': '2021-05-27',
            //    'rates': {
            //        'EUR': 1,
            //        'BRL': 6.448241,
            //        'GBP': 0.860294,
            //        'AUD': 1.574412,
            //        'USD': 1.21865
            //    }
            //}");

            //            var test = JsonConvert.DeserializeObject<Models.ExchangeRates>(o.ToString());

            var uri = new Uri($"{_baseUrl}/latest?access_key={_apiKey}&base = {baseCurrencySymbol} &symbols={conversionCurrencySymbols}");
            var exchangeRatesApiResponse = await _apiBaseService.ExecuteRequest<Models.ExchangeRates>(_httpClient, uri);

            var exchangeRates = _mapper.Map<Core.Models.ExchangeRates>(exchangeRatesApiResponse);

            return exchangeRates;
        }
    }
}
