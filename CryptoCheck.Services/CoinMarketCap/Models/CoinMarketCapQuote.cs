using Newtonsoft.Json;
using System;

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
        public CurrencyQuote CurrencyQuote { get; set; }
    }

    public class CurrencyQuote
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("volume_24h")]
        public decimal Volume24H { get; set; }

        [JsonProperty("percent_change_1h")]
        public decimal PercentChange1H { get; set; }

        [JsonProperty("percent_change_24h")]
        public decimal PercentChange24H { get; set; }

        [JsonProperty("percent_change_7d")]
        public decimal PercentChange7D { get; set; }

        [JsonProperty("percent_change_30d")]
        public decimal PercentChange30D { get; set; }

        [JsonProperty("percent_change_60d")]
        public decimal PercentChange60D { get; set; }

        [JsonProperty("percent_change_90d")]
        public decimal PercentChange90D { get; set; }

        [JsonProperty("market_cap")]
        public decimal MarketCap { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }
    }
}
