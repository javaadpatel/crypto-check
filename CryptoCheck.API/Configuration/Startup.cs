using CryptoCheck.API.Configuration;
using CryptoCheck.Core.Contracts;
using CryptoCheck.Services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using AzureFunctions.Extensions.Swashbuckle;
using System.Reflection;
using System.Linq;
using FluentValidation;
using CryptoCheck.Core.Models;
using CryptoCheck.Core.Validators;
using CryptoCheck.Core.PollyPolicies;
using Microsoft.Extensions.Logging;
using CryptoCheck.AutoMapper.Builders;
using CryptoCheck.Services.ExchangeRatesApi;
using CryptoCheck.Services.CoinMarketCap;

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
                x.DefaultRequestHeaders.Add("User-Agent", Environment.GetEnvironmentVariable("httpClient:userAgent"));
                x.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", Environment.GetEnvironmentVariable("coinMarketCapApi:apiKey"));
            })
            .AddPolicyHandler((s, request) => PollyPolicies.GetRetryPolicy(
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy:retryCount")),
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy:timeoutInSeconds")),
                s.GetService<ILogger<ICryptoPriceService>>())
            );

            builder.Services.AddHttpClient<IExchangeRatesService, ExchangeRatesApiService>(x =>
            {
                x.DefaultRequestHeaders.Add("User-Agent", Environment.GetEnvironmentVariable("httpClient:userAgent"));
            })
            .AddPolicyHandler((s, request) => PollyPolicies.GetRetryPolicy(
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy:retryCount")),
                int.Parse(Environment.GetEnvironmentVariable("retryPolicy:timeoutInSeconds")),
                s.GetService<ILogger<IExchangeRatesService>>())
            );

            //register validators
            builder.Services.AddTransient<IValidator<CryptoQuoteRequest>, CryptoQuoteRequestValidator>();

            //register automapper
            builder.Services.AddSingleton(sp => MapperBuilder.Mapper);
            builder.Services.AddSingleton(sp => MapperBuilder.ConfigurationProvider);
        }
    }
}
