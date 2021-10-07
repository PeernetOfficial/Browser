using System.Net.Http;

namespace Peernet.Browser.Infrastructure.Http
{
    internal interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}