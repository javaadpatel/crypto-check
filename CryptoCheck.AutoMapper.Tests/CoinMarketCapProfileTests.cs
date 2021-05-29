using AutoMapper;
using CryptoCheck.AutoMapper.Profiles;
using CryptoCheck.Core.Models;
using NUnit.Framework;
using System;

namespace CryptoCheck.AutoMapper.Tests
{
    [TestFixture]
    public class CoinMarketCapProfileTests
    {
        private MapperConfiguration _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MapperConfiguration(cfg => cfg.AddProfile<CoinMarketCapProfile>());
        }

        [Test]
        public void GivenMappingProfile_WhenMappingFromCryptoCurrencyQuoteDate_ShouldMapCorrectlyToCryptoCurrencyPrice()
        {
            //arrange
            var mapper = _sut.CreateMapper();
            var timeNow = DateTimeOffset.UtcNow;
            var quoteData = new Services.CoinMarketCap.Models.CryptoCurrencyQuoteData()
            {
                Name = "Bitcoin",
                Symbol = "BTC",
                Quote = new Services.CoinMarketCap.Models.Quote
                {
                    CurrencyQuote = new Services.CoinMarketCap.Models.CurrencyQuote
                    {
                        Price = 100.12m
                    }
                },
                LastUpdated = timeNow
            };

            var cryptoCurrencyPrice = new CryptoCurrencyPrice()
            {
                CryptoSymbol = "BTC",
                Name = "Bitcoin",
                Price = 100.12m,
                LastUpdated = timeNow
            };

            //act
            var result = mapper.Map<CryptoCurrencyPrice>(quoteData);

            //assert
            Assert.AreEqual("BTC", result.CryptoSymbol);
            Assert.That(timeNow, Is.EqualTo(result.LastUpdated).Within(TimeSpan.FromMinutes(1.0)));
            Assert.AreEqual(100.12, result.Price);
            Assert.AreEqual("Bitcoin", result.Name);
        }
    }
}