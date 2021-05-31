using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface IApiBaseService
    {
        Task<string> ExecuteRequest(HttpClient httpClient, Uri uri);
    }
}
