using System;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoCheck.Core.Contracts
{
    public interface ICacheService
    {
        Task<T> GetItemAsync<T>(string cacheKey, Func<Task<T>> cacheRefreshFunc, CancellationToken cancellationToken = default);

        Task SetItemAsync<T>(string cacheKey, T item, CancellationToken cancellationToken);
    }
}
