using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Models
{
    public class CryptoQuote
    {
        /// <summary>
        /// The crytocurrency name (eg. Bitcoin, Ethereum)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The cryptocurrency symbol (eg. BTC, ETH)
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// The price of the cryptocurrency quoted in multiple currencies
        /// </summary>
        public Dictionary<string, decimal> CurrencyQuotes { get; set; }

        /// <summary>
        /// Indicates if the quote was a cached quote or a newly generated quote
        /// </summary>
        public bool IsCachedResponse { get; set; }

        /// <summary>
        /// The time the quote was issued, this can either be the time the request was made or a previous timestamp if
        /// the quote was served from the cache
        /// </summary>
        public DateTime IssuedAt { get; set; }
    }
}
