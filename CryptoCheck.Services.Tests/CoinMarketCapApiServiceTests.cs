using AutoMapper;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using CryptoCheck.Services.CoinMarketCap;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
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
        private ICacheService _cacheService;
        private ILogger<ICryptoPriceService> _logger;
        private string _cryptoCurrencySymbol;

        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient(new FakeHttpMessageHandler()) { BaseAddress = new Uri("https://localhost") };

            _mapper = Substitute.For<IMapper>();
            _configuration = Substitute.For<IConfiguration>();

            _configuration["coinMarketCapApi_baseUrl"].Returns("https://notcoinmarketcap.com/v1/");
            _configuration["coinMarketCapApi_auxFields"].Returns("is_active");
            _configuration["coinMarketCapApi_baseCurrencySymbol"].Returns("EUR");

            _apiBaseService = Substitute.For<IApiBaseService>();
            _cacheService = Substitute.For<ICacheService>();
            _logger = Substitute.For<ILogger<ICryptoPriceService>>();
            _cryptoCurrencySymbol = "BTC";

            _sut = new CoinMarketCapApiService(_httpClient, _mapper, _configuration, _apiBaseService, _cacheService, _logger);
        }

        [Test]
        public async Task GivenCryptoSymbol_WhenFetchingCryptoPrice_ShouldReturnAWellFormedCryptoCurrencyPriceModel()
        {
            //arrange
            var timeNow = DateTime.UtcNow;

            var quoteDataString = @"
{
    'status': {
        'timestamp': '2021-05-29T13:31:03.066Z',
        'error_code': 0,
        'error_message': null,
        'elapsed': 16,
        'credit_count': 1,
        'notice': null
    },
    'data': {
        'BTC': {
            'id': 1,
            'name': 'Bitcoin',
            'symbol': 'BTC',
            'slug': 'bitcoin',
            'is_active': 1,
            'last_updated': '2021-05-29T13:30:02.000Z',
            'quote': {
                'EUR': {
                    'price': 28153.99259921275,
                    'volume_24h': 38083178692.10364,
                    'percent_change_1h': -1.96592175,
                    'percent_change_24h': -7.39095393,
                    'percent_change_7d': -12.42207251,
                    'percent_change_30d': -36.40293262,
                    'percent_change_60d': -42.05679688,
                    'percent_change_90d': -23.08735837,
                    'market_cap': 527073175923.26245,
                    'last_updated': '2021-05-29T13:30:12.000Z'
                }
            }
        }
    }
}
";

            var cryptoCurrencyPrice = new CryptoCurrencyPrice()
            {
                CryptoSymbol = "BTC",
                CurrencySymbol = "USD",
                Name = "Bitcoin",
                Price = 100.12m,
                LastUpdated = timeNow
            };

            _apiBaseService
                .ExecuteRequest(_httpClient, Arg.Any<Uri>())
                .Returns(quoteDataString);

            _mapper
                .Map<CryptoCurrencyPrice>(Arg.Any<CoinMarketCap.Models.CryptoCurrencyQuoteData>())
                .Returns(cryptoCurrencyPrice);

            //act
            var result = await _sut.GetCryptoPriceAsync(_cryptoCurrencySymbol);

            //assert
            Assert.IsNotNull(result);

            await _apiBaseService
                .Received(1)
                .ExecuteRequest(_httpClient, Arg.Any<Uri>());

            _mapper
                .Received(1)
                .Map<CryptoCurrencyPrice>(Arg.Any<CoinMarketCap.Models.CryptoCurrencyQuoteData>());

        }
    }
}
