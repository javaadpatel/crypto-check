using AutoMapper;

namespace CryptoCheck.AutoMapper.Profiles
{
    public class CoinMarketCapProfile : Profile
    {
        public CoinMarketCapProfile()
        {
            CreateMap<Core.Models.CryptoCurrencyPrice, Services.CoinMarketCap.Models.CryptoCurrencyQuoteData>()
                .ReverseMap()
                .ForMember(
                    dest => dest.Price,
                    opt => opt.MapFrom(src => src.Quote.CurrencyQuote.Price)
                )
                .ForMember(
                    dest => dest.CryptoSymbol,
                    opt => opt.MapFrom(src => src.Symbol)
                );


            CreateMap<Core.Models.CryptoCurrency, Services.CoinMarketCap.Models.CoinMarketCapCryptoCurrencyMap>()
                .ReverseMap();
        }
    }
}
