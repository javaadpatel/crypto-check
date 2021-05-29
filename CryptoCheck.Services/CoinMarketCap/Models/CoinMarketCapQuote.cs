using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.Services.CoinMarketCap.Models
{
    public class CryptoCurrencyQuoteData
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("is_active")]
        public long IsActive { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }

        [JsonProperty("quote")]
        public Quote Quote { get; set; }
    }

    public class Quote
    {
        [JsonProperty("USD")]
        public CurrencyQuote CurrencyQuote { get; set; }
    }

    public class CurrencyQuote
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("volume_24h")]
        public double Volume24H { get; set; }

        [JsonProperty("percent_change_1h")]
        public double PercentChange1H { get; set; }

        [JsonProperty("percent_change_24h")]
        public double PercentChange24H { get; set; }

        [JsonProperty("percent_change_7d")]
        public double PercentChange7D { get; set; }

        [JsonProperty("percent_change_30d")]
        public double PercentChange30D { get; set; }

        [JsonProperty("percent_change_60d")]
        public double PercentChange60D { get; set; }

        [JsonProperty("percent_change_90d")]
        public double PercentChange90D { get; set; }

        [JsonProperty("market_cap")]
        public double MarketCap { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
