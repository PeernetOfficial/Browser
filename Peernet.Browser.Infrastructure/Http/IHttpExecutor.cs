using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Http
{
    internal interface IHttpExecutor
    {
        Task<T> GetResult<T>(
            HttpMethod method,
            string relativePath,
            Dictionary<string, string> queryParameters = null,
            JsonContent content = null);
    }
}