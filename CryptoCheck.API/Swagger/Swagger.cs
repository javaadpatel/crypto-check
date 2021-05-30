﻿using AzureFunctions.Extensions.Swashbuckle;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.API.Swagger
{
    public class Swagger
    {
        [SwaggerIgnore]
        [FunctionName("Swagger")]
        public Task<HttpResponseMessage> GetJson(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/json")] HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient
        )
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerDocumentResponse(req));
        }

        [SwaggerIgnore]
        [FunctionName("SwaggerUi")]
        public Task<HttpResponseMessage> GetUi(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "swagger/ui")] HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient
        )
        {
            return Task.FromResult(swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json"));
        }

    }
}
