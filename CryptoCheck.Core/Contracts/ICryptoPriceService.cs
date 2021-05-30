using CryptoCheck.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface ICryptoPriceService
    {
        Task<CryptoCurrencyPrice> GetCryptoPriceAsync(string symbol);

        Task<IList<CryptoCurrency>> GetAllCryptoCurrencies();
    }
}
