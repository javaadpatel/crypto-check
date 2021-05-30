using CryptoCheck.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface ICryptoPriceService
    {
        Task<CryptoCurrencyPrice> GetCryptoPriceAsync(string symbol);

        Task<IList<CryptoCurrency>> GetAllCryptoCurrencies();
    }
}
