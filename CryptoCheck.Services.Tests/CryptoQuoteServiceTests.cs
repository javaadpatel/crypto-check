using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCheck.Services.Tests
{
    [TestFixture]
    public class CryptoQuoteServiceTests
    {
        private CryptoQuoteService _sut;
        private ICryptoPriceService _cryptoPriceService;
        private IExchangeRatesService _exchangeRateService;
        private IConfiguration _configuration;
        private string _cryptoCurrencySymbol;

        [SetUp]
        public void Setup()
        {
            _cryptoPriceService = Substitute.For<ICryptoPriceService>();
            _exchangeRateService = Substitute.For<IExchangeRatesService>();
            _configuration = Substitute.For<IConfiguration>();
            _cryptoCurrencySymbol = "BTC";

            _sut = new CryptoQuoteService(
                _cryptoPriceService,
                _exchangeRateService,
                _configuration
            );
        }

        [Test]
        public async Task GivenCryptoCurrencyPricesAndExchangeRates_WhenGeneratingAQuote_ShouldCalculateTheCorrectQuotes()
        {
            //arrange
            var quoteRequest = new CryptoQuoteRequest(_cryptoCurrencySymbol);

            _cryptoPriceService
                .GetCryptoPriceAsync(_cryptoCurrencySymbol)
                .Returns(new CryptoCurrencyPrice
                {
                    CryptoSymbol = _cryptoCurrencySymbol,
                    CurrencySymbol = "USD",
                    LastUpdated = DateTime.Now,
                    Name = "Bitcoin",
                    Price = 39488.91493854606
                });

            _exchangeRateService
                .GetExchangeRatesAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(new ExchangeRates
                {
                    BaseSymbol = "EUR",
                    Timestamp = 1622131924,
                    Date = DateTimeOffset.Now,
                    Rates = new Dictionary<string, double>
                    {
                        { "EUR", 1 },
                        { "BRL", 6.424691 },
                        { "GBP", 0.860088 },
                        { "AUD", 1.576404},
                        {"USD", 1.219222}
                    }
                });

            //act
            var result = await _sut.GenerateQuoteAsync(quoteRequest);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_cryptoCurrencySymbol, result.Symbol);
            Assert.That(DateTime.UtcNow, Is.EqualTo(result.IssuedAt).Within(TimeSpan.FromMinutes(1.0)));

            var expectedPrices = new Dictionary<string, double>
            {
                { "EUR", 32388.617445014988 },
                { "BRL", 208086.8590014308 },
                { "GBP", 27857.061201048051 },
                { "AUD", 51057.5460947914},
                { "USD", 39488.914938546062}
            };

            Assert.AreEqual(expectedPrices, result.CurrencyQuotes);
        }
    }
}