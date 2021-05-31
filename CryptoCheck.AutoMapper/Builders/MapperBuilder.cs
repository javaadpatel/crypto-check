using AutoMapper;
using CryptoCheck.AutoMapper.Profiles;
using System;

namespace CryptoCheck.AutoMapper.Builders
{
    public static class MapperBuilder
    {
        private static readonly Lazy<IConfigurationProvider> _configurationProvider =
            new Lazy<IConfigurationProvider>(DefaultConfiguration, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static IConfigurationProvider ConfigurationProvider
        {
            get
            {
                return _configurationProvider.Value;
            }
        }

        private static readonly Lazy<IMapper> _mapper =
            new Lazy<IMapper>(DefaultMapper, System.Threading.LazyThreadSafetyMode.ExecutionAndPublication);

        public static IMapper Mapper
        {
            get
            {
                return _mapper.Value;
            }
        }


        private static IConfigurationProvider DefaultConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CoinMarketCapProfile>();
                cfg.AddProfile<ExchangeRatesApiProfile>();
            });

            return configuration;
        }

        private static IMapper DefaultMapper()
        {
            var mapper = ConfigurationProvider.CreateMapper();
            return mapper;
        }
    }
}
