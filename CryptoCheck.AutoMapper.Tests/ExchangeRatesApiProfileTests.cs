using AutoMapper;
using CryptoCheck.AutoMapper.Profiles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.AutoMapper.Tests
{
    [TestFixture]
    public class ExchangeRatesApiProfileTests
    {
        private MapperConfiguration _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new MapperConfiguration(cfg => cfg.AddProfile<ExchangeRatesApiProfile>());
        }

        [Test]
        public void GivenMappingProfile_WhenMappingFromExchangeRatesApiModelto_ShouldMapCorrectlyToExchangeRates()
        {
            //arrange
            var mapper = _sut.CreateMapper();
            var timeNow = DateTimeOffset.UtcNow;
            var currencyRates = new Dictionary<string, decimal>
            {
                {"EUR", 1 },
                { "USD", 1.225m}
            };
            var exchangeRatesApiResponseModel = new Services.ExchangeRatesApi.Models.ExchangeRates
            {
                Base = "EUR",
                Date = timeNow,
                Success = true,
                Timestamp = 12314513,
                Rates = currencyRates
            };

            //act
            var result = mapper.Map<Core.Models.ExchangeRates>(exchangeRatesApiResponseModel);

            //assert
            Assert.AreEqual("EUR", result.BaseSymbol);
            Assert.That(timeNow, Is.EqualTo(result.Date).Within(TimeSpan.FromMinutes(1.0)));
            Assert.AreEqual(currencyRates, result.Rates);
            Assert.AreEqual(12314513, result.Timestamp);
        }
    }
}
