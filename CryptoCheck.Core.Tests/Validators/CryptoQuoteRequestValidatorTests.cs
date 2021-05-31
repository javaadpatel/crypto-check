using CryptoCheck.Core.Models;
using CryptoCheck.Core.Validators;
using NUnit.Framework;

namespace CryptoCheck.Core.Tests.Validators
{
    [TestFixture]
    public class CryptoQuoteRequestValidatorTests
    {
        private CryptoQuoteRequestValidator _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new CryptoQuoteRequestValidator();
        }

        [TestCase("BTC")]
        [TestCase("btc")]
        [TestCase("ETH")]
        [TestCase("eth")]
        [TestCase("SALT")]
        public void GivenValidSymbol_WhenValidating_ShouldPassValidation(string symbol)
        {
            //arrange
            var quoteRequest = new CryptoQuoteRequest(symbol);

            //act
            var validationResult = _sut.Validate(quoteRequest);

            //assert
            Assert.IsTrue(validationResult.IsValid);
        }

        [TestCase("BTC1")]
        [TestCase("111")]
        [TestCase("BT!")]
        public void GivenInvalidSymbol_WhenValidating_ShouldFailValidation(string symbol)
        {
            //arrange
            var quoteRequest = new CryptoQuoteRequest(symbol);

            //act
            var validationResult = _sut.Validate(quoteRequest);

            //assert
            Assert.IsFalse(validationResult.IsValid);
        }
    }
}
