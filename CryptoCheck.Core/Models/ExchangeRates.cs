using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.Core.Models
{
    public class ExchangeRates
    {
        public long Timestamp { get; set; }

        /// <summary>
        /// The symbol of the currency used as the base currency (eg. EUR)
        /// </summary>
        public string BaseSymbol { get; set; }

        public DateTimeOffset Date { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
    }
}
