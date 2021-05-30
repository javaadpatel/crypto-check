using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace CryptoCheck.Core.PollyPolicies
{
    public static class PollyPolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, int timeoutSeconds, ILogger logger)
        {
            Random jitterer = new Random();

            var retryWithJitterPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(retryCount,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)),
                    onRetry: (exception, calculatedWaitDuration, retryAttempt, context) =>
                    {
                        logger.LogError($"Retrying request, current retry attempt {retryAttempt}. " +
                                        $"Response status code is {exception.Result.StatusCode}." +
                                        $"Response content is {exception.Result.Content}." +
                                        $"Response error is {exception.Exception.Message}." +
                                        $"Retrying in {calculatedWaitDuration.TotalSeconds} seconds.");
                    });

            return retryWithJitterPolicy;
        }
    }
}
