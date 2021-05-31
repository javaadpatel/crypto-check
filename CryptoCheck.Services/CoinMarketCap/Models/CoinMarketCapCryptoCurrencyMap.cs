namespace CryptoCheck.Services.CoinMarketCap.Models
{
    public class CoinMarketCapCryptoCurrencyMap
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Slug { get; set; }
        public int Rank { get; set; }
    }
}
