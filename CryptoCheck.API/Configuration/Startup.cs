using AzureFunctions.Extensions.Swashbuckle;
using CryptoCheck.API.Configuration;
using CryptoCheck.AutoMapper.Builders;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Core.Models;
using CryptoCheck.Core.PollyPolicies;
using CryptoCheck.Core.Validators;
using CryptoCheck.Services;
using CryptoCheck.Services.CoinMarketCap;
using CryptoCheck.Services.ExchangeRatesApi;
using FluentValidation;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace CryptoCheck.API.Configuration
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //configure swagger
            builder.AddSwashBuckle(Assembly.GetExecutingAssembly(), opts =>
            {
                opts.ConfigureSwaggerGen = swg =>
                {
                    var document = swg.SwaggerGeneratorOptions.SwaggerDocs.Single().Value;
                    document.Title = "CryptoCheck";
                    document.Description = "CryptoCheck API for getting cryptocurrency quotes";

                    swg.EnableAnnotations();
                };
            });

            //register services
            builder
                .Services
                .AddSingleton<ICryptoQuoteService, CryptoQuoteService>()
                .AddSingleton<IApiBaseService, ApiBaseService>();

            builder.Services.AddHttpClient<ICryptoPriceService, CoinMarketCapApiService>(x =>
            {
                x.DefaultRequestHeaders.Add("User-Agent", Environment.GetEnvironmentVariable("httpClient_userAgent"));
                x.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", Environment.GetEnvironmentVariable("coinMarketCapApi_apiKey"));
            })
            .AddPolicyHandler((s, request) => PollyPolicies.GetRetryPolicy(
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy_retryCount")),
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy_timeoutInSeconds")),
                s.GetService<ILogger<ICryptoPriceService>>())
            );

            builder.Services.AddHttpClient<IExchangeRatesService, ExchangeRatesApiService>(x =>
            {
                x.DefaultRequestHeaders.Add("User-Agent", Environment.GetEnvironmentVariable("httpClient_userAgent"));
            })
            .AddPolicyHandler((s, request) => PollyPolicies.GetRetryPolicy(
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy_retryCount")),
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy_timeoutInSeconds")),
                s.GetService<ILogger<IExchangeRatesService>>())
            );

            //register validators
            builder.Services.AddTransient<IValidator<CryptoQuoteRequest>, CryptoQuoteRequestValidator>();

            //register automapper
            builder.Services.AddSingleton(sp => MapperBuilder.Mapper);
            builder.Services.AddSingleton(sp => MapperBuilder.ConfigurationProvider);

            //register cache
            builder.Services.AddSingleton<ICacheService, CacheService>();
            if (Debugger.IsAttached)
            {
                builder.Services.AddDistributedMemoryCache();
            }
            else
            {
                builder.Services.AddDistributedMemoryCache();
                //TODO: implement when using redis
                //services.AddStackExchangeRedisCache(options =>
                //{
                //    options.Configuration = Environment.GetEnvironmentVariable("cache:connectionString");
                //    options.InstanceName = Environment.GetEnvironmentVariable("cache:instanceName");
                //});
            }
        }
    }
}
