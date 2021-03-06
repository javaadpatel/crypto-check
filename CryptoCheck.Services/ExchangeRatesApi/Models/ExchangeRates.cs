using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CryptoCheck.Services.ExchangeRatesApi.Models
{
    public class ExchangeRates
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
