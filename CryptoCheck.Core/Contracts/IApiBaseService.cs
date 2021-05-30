﻿using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface IApiBaseService
    {
        Task<T> ExecuteRequest<T>(HttpClient httpClient, Uri uri);
        Task<string> ExecuteRequest(HttpClient httpClient, Uri uri);
    }
}
