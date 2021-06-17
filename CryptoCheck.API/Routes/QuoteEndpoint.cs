using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using CryptoCheck.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.API.Routes
{
    public class QuoteEndpoint
    {
        private readonly ICryptoQuoteService _cryptoQuoteService;
        private readonly ICryptoPriceService _cryptoPriceService;
        private readonly IValidator<CryptoQuoteRequest> _quoteRequestValidator;

        public QuoteEndpoint(CryptoQuoteServiceResolver cryptoQuoteServiceResolver, ICryptoPriceService cryptoPriceService, IValidator<CryptoQuoteRequest> quoteRequestValidator)
        {
            _cryptoQuoteService = cryptoQuoteServiceResolver("quoteWithValidationService") ?? throw new ArgumentNullException(nameof(cryptoQuoteServiceResolver));
            _cryptoPriceService = cryptoPriceService ?? throw new ArgumentNullException(nameof(cryptoPriceService));
            _quoteRequestValidator = quoteRequestValidator ?? throw new ArgumentNullException(nameof(quoteRequestValidator));
        }

        [SwaggerOperation("Get cryptocurrency quote")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(CryptoQuote))]
        [FunctionName(nameof(QuoteEndpoint) + "_" + nameof(Quote))]
        public async Task<IActionResult> Quote(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "quote/{symbol}")] HttpRequestMessage httpRequest,
            string symbol,
            ILogger logger)
        {
            var cryptoQuoteRequest = new CryptoQuoteRequest(symbol);

            var validationResult = _quoteRequestValidator.Validate(cryptoQuoteRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            try
            {
                var quote = await _cryptoQuoteService.GenerateQuoteAsync(cryptoQuoteRequest);

                if (!string.IsNullOrWhiteSpace(quote.ErrorMessage))
                    return new BadRequestObjectResult(new { Message = quote.ErrorMessage});
                
                return new OkObjectResult(quote);
            }
            catch
            {
                return new BadRequestObjectResult(new { Message = $"Unable to generate quote for {cryptoQuoteRequest.Symbol}" });
            }

        }

        [SwaggerOperation("Search for cryptocurrency")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(IList<CryptoCurrency>))]
        [FunctionName(nameof(QuoteEndpoint) + "_" + nameof(Search))]
        public async Task<IActionResult> Search(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "search/{searchTerm}")] HttpRequestMessage httpRequest,
            string searchTerm,
            ILogger logger
        )
        {
            List<CryptoCurrency> possibleCryptoCurrencies = await GetCryptoCurrenciesBySymbol(searchTerm);

            return new OkObjectResult(possibleCryptoCurrencies);
        }

        private async Task<List<CryptoCurrency>> GetCryptoCurrenciesBySymbol(string searchTerm)
        {
            var allCryptoCurrencies = await _cryptoPriceService.GetAllCryptoCurrencies();

            var possibleCryptoCurrencies = allCryptoCurrencies.Where(c => c.Symbol.Contains(searchTerm.ToUpper())).ToList();

            return possibleCryptoCurrencies;
        }

    }
}
