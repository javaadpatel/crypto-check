using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.Core.Models
{
    public class CryptoCurrencyPrice
    {
        public string Name { get; set; }

        /// <summary>
        /// The symbol of the cryptocurrency (eg. BTC)
        /// </summary>
        public string CryptoSymbol { get; set; }

        /// <summary>
        /// The symbol of the currency that the price was issued in (eg. USD)
        /// </summary>
        public string CurrencySymbol { get; set; }

        public DateTimeOffset LastUpdated { get; set; }

        public decimal Price { get; set; }
    }
}
