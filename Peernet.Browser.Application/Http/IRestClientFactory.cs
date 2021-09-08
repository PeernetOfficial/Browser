using RestSharp;

namespace Peernet.Browser.Application.Http
{
    public interface IRestClientFactory
    {
        RestClient CreateRestClient();
    }
}