using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Services
{
    //Decorated CryptoQuoteService, adding validation logic to check if the cryptocurrency is a valid one
    public class CryptoQuoteWithValidationService : ICryptoQuoteService
    {
        private ICryptoQuoteService _baseQuoteService;
        private readonly ICryptoPriceService _cryptoPriceService;

        public CryptoQuoteWithValidationService(CryptoQuoteServiceResolver cryptoQuoteServiceResolver, ICryptoPriceService cryptoPriceService)
        {
            _baseQuoteService = cryptoQuoteServiceResolver("quoteService") ?? throw new ArgumentNullException(nameof(cryptoQuoteServiceResolver));
            _cryptoPriceService = cryptoPriceService ?? throw new ArgumentNullException(nameof(cryptoPriceService));
        }

        public async Task<CryptoQuote> GenerateQuoteAsync(CryptoQuoteRequest cryptoQuoteRequest)
        {
            //first check if the cryptocurrency symbol is legitimate
            if (!(await GetCryptoCurrenciesBySymbol(cryptoQuoteRequest.Symbol)).Any())
            {
                return new CryptoQuote
                { 
                    ErrorMessage = $"No cryptocurrency with the symbol '{cryptoQuoteRequest.Symbol}' could be found. Please check your spelling and try again." 
                };
            }

            return await _baseQuoteService.GenerateQuoteAsync(cryptoQuoteRequest);
        }

        private async Task<List<CryptoCurrency>> GetCryptoCurrenciesBySymbol(string searchTerm)
        {
            var allCryptoCurrencies = await _cryptoPriceService.GetAllCryptoCurrencies();

            var possibleCryptoCurrencies = allCryptoCurrencies.Where(c => c.Symbol.Contains(searchTerm.ToUpper())).ToList();

            return possibleCryptoCurrencies;
        }
    }
}
