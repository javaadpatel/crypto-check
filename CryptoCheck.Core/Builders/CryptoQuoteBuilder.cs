using CryptoCheck.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.Core.Builders
{
    public sealed class CryptoQuoteBuilder : FunctionalBuilder<CryptoQuote, CryptoQuoteBuilder>
    {
        public CryptoQuoteBuilder Named(string cryptoCurrencyName) => Do(c => c.Name = cryptoCurrencyName);

        public CryptoQuoteBuilder WithSymbol(string cryptoCurrencySymbol) => Do(c => c.Symbol = cryptoCurrencySymbol);

        public CryptoQuoteBuilder WithQuotes(Dictionary<string, decimal> currencyQuotes) => Do(c => c.CurrencyQuotes = currencyQuotes);

        public CryptoQuoteBuilder IssuedAt(DateTime issuedAt) => Do(c => c.IssuedAt = issuedAt);
    }

    public static class CryptoQuoteBuilderExtension
    {
        public static CryptoQuoteBuilder IsCachedResponse(this CryptoQuoteBuilder builder, bool isCachedResponse) => builder.Do(c => c.IsCachedResponse = isCachedResponse);
    }

}
