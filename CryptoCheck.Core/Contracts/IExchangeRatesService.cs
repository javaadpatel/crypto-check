using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface IExchangeRatesService
    {
        Task<Core.Models.ExchangeRates> GetExchangeRatesAsync(string baseCurrencySymbol, string conversionCurrencySymbols);
    }
}
