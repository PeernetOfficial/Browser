using System.Net.Http;

namespace Peernet.Browser.Application.Http
{
    public interface IHttpClientFactory
    {
        HttpClient CreateHttpClient();
    }
}