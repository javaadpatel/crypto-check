using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

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
        }
    }
}
