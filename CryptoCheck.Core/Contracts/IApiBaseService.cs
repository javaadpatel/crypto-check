using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface IApiBaseService
    {
        Task<T> ExecuteRequest<T>(HttpClient httpClient, Uri uri);
        Task<string> ExecuteRequest(HttpClient httpClient, Uri uri);
    }
}
