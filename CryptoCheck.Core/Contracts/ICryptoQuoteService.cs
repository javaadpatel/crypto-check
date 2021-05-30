using CryptoCheck.Core.Models;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface ICryptoQuoteService
    {
        Task<CryptoQuote> GenerateQuoteAsync(CryptoQuoteRequest cryptoQuoteRequest);
    }
}
