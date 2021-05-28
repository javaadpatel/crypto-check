using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using FluentValidation;
using System.Linq;

namespace CryptoCheck.API.Routes
{
    public class QuoteEndpoint
    {
        private readonly ICryptoQuoteService _cryptoQuoteService;
        private readonly IValidator<CryptoQuoteRequest> _quoteRequestValidator;

        public QuoteEndpoint(ICryptoQuoteService cryptoQuoteService, IValidator<CryptoQuoteRequest> quoteRequestValidator)
        {
            _cryptoQuoteService = cryptoQuoteService ?? throw new ArgumentNullException(nameof(cryptoQuoteService));
            _quoteRequestValidator = quoteRequestValidator ?? throw new ArgumentNullException(nameof(quoteRequestValidator));
        }

        [SwaggerOperation("Get cryptocurrency quote")]
        [ProducesResponseType((int) HttpStatusCode.OK, Type = typeof(CryptoQuote))]
        [FunctionName(nameof(QuoteEndpoint) + "_" + nameof(Quote))]
        public async Task<IActionResult> Quote(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "quote")] CryptoQuoteRequest cryptoQuoteRequest,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var validationResult = _quoteRequestValidator.Validate(cryptoQuoteRequest);

            if (!validationResult.IsValid)
            {
                return new BadRequestObjectResult(validationResult.Errors.Select(e => new {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            try
            {
                var quote = await _cryptoQuoteService.GenerateQuoteAsync(cryptoQuoteRequest);
                return new OkObjectResult(quote);
            }
            catch
            {
                //TODO: We can cross reference the symbol, maybe return from the service layer a more detailed error message to use here
                return new BadRequestObjectResult(new { Message = $"Unable to generate quote for {cryptoQuoteRequest.Symbol}" });
            }

        }
    }
}
