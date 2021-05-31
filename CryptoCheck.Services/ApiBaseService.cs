using CryptoCheck.Core.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.Services
{
    public class ApiBaseService : IApiBaseService
    {
        private readonly ILogger<IApiBaseService> _logger;

        public ApiBaseService(ILogger<IApiBaseService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> ExecuteRequest(HttpClient httpClient, Uri uri)
        {
            HttpResponseMessage response;
            string responseContent;
            HttpStatusCode responseStatusCode;
            try
            {
                response = await httpClient.GetAsync(uri);
                responseStatusCode = response.StatusCode;
                responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception occurred {ex.Message}");
                throw new Exception("Exception occurred making API request");
            }

        }
    }
}
