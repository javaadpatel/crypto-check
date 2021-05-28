using AutoMapper;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using CryptoCheck.Services.CoinMarketCap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoCheck.Services.Tests
{
    public class FakeHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }
    }

    [TestFixture]
    public class CoinMarketCapApiServiceTests
    {
        private CoinMarketCapApiService _sut;
        private HttpClient _httpClient;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IApiBaseService _apiBaseService;
        private ILogger<ICryptoPriceService> _logger;
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
            _logger = Substitute.For<ILogger<ICryptoPriceService>>();
            _cryptoCurrencySymbol = "BTC";

            _sut = new CoinMarketCapApiService(_httpClient, _mapper, _configuration, _apiBaseService, _logger);
        }

        [Test]
        public async Task GivenCryptoSymbol_WhenFetchingCryptoPrice_ShouldReturnAWellFormedCryptoCurrencyPriceModel()
        {
            //arrange
            var timeNow = DateTime.UtcNow;

            var quoteData = new CoinMarketCap.Models.CryptoCurrencyQuoteData()
            {
                Name = "Bitcoin",
                Symbol = "BTC",
                Quote = new CoinMarketCap.Models.Quote
                {
                    CurrencyQuote = new CoinMarketCap.Models.CurrencyQuote
                    {
                        Price = 100.12
                    }
                },
                LastUpdated = timeNow
            };
            var cryptoCurrencyPrice = new CryptoCurrencyPrice()
            {
                CryptoSymbol = "BTC",
                CurrencySymbol = "USD",
                Name = "Bitcoin",
                Price = 100.12,
                LastUpdated = timeNow
            };

            _apiBaseService
                .ExecuteRequest<CoinMarketCap.Models.CryptoCurrencyQuoteData>(_httpClient, Arg.Any<Uri>(), Arg.Any<string>())
                .Returns(quoteData);

            _mapper
                .Map<CryptoCurrencyPrice>(quoteData)
                .Returns(cryptoCurrencyPrice);

            //act
            var result = await _sut.GetCryptoPriceAsync(_cryptoCurrencySymbol);

            //assert
            Assert.IsNotNull(result);

            await _apiBaseService
                .Received(1)
                .ExecuteRequest<CoinMarketCap.Models.CryptoCurrencyQuoteData>(_httpClient, Arg.Any<Uri>(), Arg.Any<string>());

            _mapper
                .Received(1)
                .Map<CryptoCurrencyPrice>(quoteData);
        }
    }
}
