using AutoMapper;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using CryptoCheck.Services.ExchangeRatesApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.Services.Tests
{
    [TestFixture]
    public class ExchangeRatesApiServiceTests
    {
        private ExchangeRatesApiService _sut;
        private HttpClient _httpClient;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IApiBaseService _apiBaseService;
        private ILogger<IExchangeRatesService> _logger;
        private string _baseCurrencySymbol;
        private string _conversionCurrencySymbols;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient(new FakeHttpMessageHandler()) { BaseAddress = new Uri("https://localhost") };

            _mapper = Substitute.For<IMapper>();
            _configuration = Substitute.For<IConfiguration>();

            _configuration["exchangeRatesApi:baseUrl"].Returns("https://notexchangeratesapi.com/v1/");
            _configuration["exchangeRatesApi:apiKey"].Returns("apiKey");

            _apiBaseService = Substitute.For<IApiBaseService>();
            _logger = Substitute.For<ILogger<IExchangeRatesService>>();
            _baseCurrencySymbol = "EUR";
            _conversionCurrencySymbols = "EUR,USD,BRL";

            _sut = new ExchangeRatesApiService(_httpClient, _apiBaseService, _mapper, _configuration, _logger);
        }

        [Test]
        public async Task GivenABaseCurrencyAndConversionCurrencies_WhenGettingExchangeRate_ShouldReturnTheExchangeRates()
        {
            //arrange
            var exchangeRatesStringData = @"
{
    'success': true,
    'timestamp': 1622122024,
    'base': 'EUR',
    'date': '2021-05-27',
    'rates': {
        'EUR': 1,
        'BRL': 6.448241,
        'GBP': 0.860294,
        'AUD': 1.574412,
        'USD': 1.21865
    }
}
";
            _apiBaseService
               .ExecuteRequest(_httpClient, Arg.Any<Uri>())
               .Returns(exchangeRatesStringData);

            var exchangeRates = new ExchangeRates
            {
                BaseSymbol = "EUR",
                Rates = new Dictionary<string, decimal>
                {
                    { "EUR", 1 },
                    { "BRL", 6.424691m },
                    { "GBP", 0.860088m },
                    { "AUD", 1.576404m},
                    {"USD", 1.219222m}
                }
            };

            _mapper
                .Map<ExchangeRates>(Arg.Any<ExchangeRatesApi.Models.ExchangeRates>())
                .Returns(exchangeRates);

            //act
            var result = await _sut.GetExchangeRatesAsync(_baseCurrencySymbol, _conversionCurrencySymbols);

            //assert
            Assert.IsNotNull(result);

            await _apiBaseService
                .Received(1)
                .ExecuteRequest(_httpClient, Arg.Any<Uri>());

            _mapper
                .Received(1)
                .Map<ExchangeRates>(Arg.Any<ExchangeRatesApi.Models.ExchangeRates>());

        }
    }
}
