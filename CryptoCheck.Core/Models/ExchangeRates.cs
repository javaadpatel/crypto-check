using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.Core.Models
{
    public class ExchangeRates
    {
        public long Timestamp { get; set; }

        public string BaseSymbol { get; set; }

        public DateTimeOffset Date { get; set; }

        public Dictionary<string, double> Rates { get; set; }
    }
}
