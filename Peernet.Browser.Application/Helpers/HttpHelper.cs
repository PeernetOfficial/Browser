using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Helpers
{
    public static class HttpHelper
    {
        public static async Task<T> GetResult<T>(
            HttpClient httpClient,
            HttpMethod method,
            string relativePath,
            Dictionary<string, string> queryParameters = null,
            JsonContent content = null)
        {
            var requestPath = relativePath;

            if (queryParameters != null)
            {
                requestPath += "?" + GetQueryString(queryParameters);
            }

            var httpRequestMessage = new HttpRequestMessage(method, requestPath);

            if (content != null)
            {
                httpRequestMessage.Content = content;
            }

            var response = await httpClient.SendAsync(httpRequestMessage).ConfigureAwait(false);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent &&
                response.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpRequestException($"Unexpected response status code: {response.StatusCode}");
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var textReader = new StreamReader(stream);
            using var reader = new JsonTextReader(textReader);
            return new JsonSerializer().Deserialize<T>(reader);
        }

        private static string GetQueryString(Dictionary<string, string> queryParameters)
        {
            string queryString = null;
            foreach (var param in queryParameters)
            {
                if (queryString != null)
                {
                    queryString += "&";
                }

                queryString += $"{param.Key}={param.Value}";
            }

            return queryString;
        }
    }
}