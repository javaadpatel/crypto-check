using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace CryptoCheck.AutoMapper.Profiles
{
    public class ExchangeRatesApiProfile : Profile
    {
        public ExchangeRatesApiProfile()
        {
            CreateMap<Core.Models.ExchangeRates, Services.ExchangeRatesApi.Models.ExchangeRates>()
              .ReverseMap()
              .ForMember(
                dest => dest.BaseSymbol,
                opt => opt.MapFrom(x => x.Base)
                );
        }
    }
}
