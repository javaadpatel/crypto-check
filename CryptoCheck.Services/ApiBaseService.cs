using CryptoCheck.Core.Contracts;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
                //_logger.LogError($"Exception occurred {ex.Message}");
                throw new Exception("Exception occurred making API request");
            }

        }

        public async Task<T> ExecuteRequest<T>(HttpClient httpClient, Uri uri)
        {
            HttpResponseMessage response;
            string responseContent;
            HttpStatusCode responseStatusCode;
            T result;
            try
            {
                response = await httpClient.GetAsync(uri);
                responseStatusCode = response.StatusCode;
                responseContent = await response.Content.ReadAsStringAsync();

                ProcessResponse(response.IsSuccessStatusCode, responseStatusCode, responseContent);
                result = JsonConvert.DeserializeObject<T>(responseContent);

                return result;
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception occurred {ex.Message}");
                throw new Exception("Exception occurred making API request");
            }

        }

        private void ProcessResponse(bool isSuccessful, HttpStatusCode responseStatusCode, string responseContent)
        {
            if (!isSuccessful)
            {
                if (responseStatusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError("Request error: Internal Server Error. Unknown exception from Takealot side");
                    throw new Exception("Unknown error.");
                }
                else if (responseStatusCode == 0)
                {
                    _logger.LogError("Request error. Please check takealotConfig.baseUri");
                    throw new Exception(responseContent);
                }
                else if (responseStatusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogError("Request error: Bad Request. {message}", responseContent);
                    throw new Exception(responseContent);
                }
                else if (responseStatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Request error: Unauthorized. Please check your API Keys");
                    throw new Exception();
                }
            }
        }
    }
}
