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
        public void GivenMappingProfile_WhenMappingFromCryptoCurrencyQuoteData_ShouldMapCorrectlyToCryptoCurrencyPrice()
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

        [Test]
        public void GivenMappingProfile_WhenMappingFromCryptoCurrencyMap_ShouldMapCorrectlyToCryptoCurrency()
        {
            //arrange
            var mapper = _sut.CreateMapper();
            var cryptoCurrencyMap = new Services.CoinMarketCap.Models.CoinMarketCapCryptoCurrencyMap
            {
                Id = 1,
                Name = "Bitcoin",
                Rank = 1,
                Slug = "btc",
                Symbol = "btc"
            };

            var expectedCryptoCurrency = new CryptoCurrency
            {
                Name = "Bitcoin",
                Symbol = "btc"
            };

            //act
            var result = mapper.Map<CryptoCurrency>(cryptoCurrencyMap);

            //assert
            Assert.AreEqual(expectedCryptoCurrency.Name, result.Name);
            Assert.AreEqual(expectedCryptoCurrency.Symbol, result.Symbol);
        }
    }
}