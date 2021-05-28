using AutoMapper;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Services.ExchangeRatesApi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
        private string _cryptoCurrencySymbol;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient(new FakeHttpMessageHandler()) { BaseAddress = new Uri("https://localhost") };

            _mapper = Substitute.For<IMapper>();
            _configuration = Substitute.For<IConfiguration>();

            _configuration["coinMarketCapApi:baseUrl"].Returns("https://notcoinmarketcap.com/v1/");
            _configuration["coinMarketCapApi:auxFields"].Returns("is_active");


            _apiBaseService = Substitute.For<IApiBaseService>();
            _logger = Substitute.For<ILogger<IExchangeRatesService>>();
            _cryptoCurrencySymbol = "BTC";

            _sut = new ExchangeRatesApiService(_httpClient, _apiBaseService, _mapper, _configuration, _logger);
        }

        //TODO: finish this test

        [Test]
        public void Test()
        {
            Assert.Pass();
        }
    }
}
