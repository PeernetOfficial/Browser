using Newtonsoft.Json;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Http
{
    internal class HttpExecutor : IHttpExecutor
    {
        private readonly Lazy<HttpClient> httpClientLazy;
        private readonly object lockObject = new();
        private readonly INotificationsManager notificationsManager;

        public HttpExecutor(IHttpClientFactory httpClientFactory, INotificationsManager notificationsManager)
        {
            this.notificationsManager = notificationsManager;
            httpClientLazy = new Lazy<HttpClient>(httpClientFactory.CreateHttpClient);
        }

        public T GetResult<T>(HttpMethod method,
            string relativePath,
            Dictionary<string, string> queryParameters = null,
            HttpContent content = null,
            bool suppressErrorNotification = false)
        {
            var httpRequestMessage = PrepareMessage(relativePath, method, queryParameters, content);
            var response = httpClientLazy.Value.Send(httpRequestMessage);

            return GetFromResponseMessage<T>(response, suppressErrorNotification);
        }

        public async Task<T> GetResultAsync<T>(
            HttpMethod method,
            string relativePath,
            Dictionary<string, string> queryParameters = null,
            HttpContent content = null,
            bool suppressErrorNotification = false)
        {
            var httpRequestMessage = PrepareMessage(relativePath, method, queryParameters, content);
            var response = await httpClientLazy.Value.SendAsync(httpRequestMessage);

            return GetFromResponseMessage<T>(response, suppressErrorNotification);
        }

        private static T Deserialize<T>(Stream stream)
        {
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

        private static HttpRequestMessage PrepareMessage(string relativePath, HttpMethod method, Dictionary<string, string> queryParameters, HttpContent content)
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

            return httpRequestMessage;
        }

        private T GetFromResponseMessage<T>(HttpResponseMessage response, bool suppressErrorNotification)
        {
            lock (lockObject)
            {
                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent &&
                    response.StatusCode != HttpStatusCode.Created && !response.RequestMessage.RequestUri.ToString().EndsWith("status"))
                {
                    var content = response.Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    var responseBody = string.IsNullOrEmpty(content) ? "(empty response)" : content;
                    var details =
                        $"{response.RequestMessage.Method} {response.RequestMessage.RequestUri} \n" +
                        $"{response.RequestMessage.Content} \n" +
                        $"Result: HTTP {response.StatusCode} \n" +
                        $"{responseBody}";

                    if (!suppressErrorNotification)
                    {
                        notificationsManager.Notifications.Add(new(
                        $"Unexpected response status code: {response.StatusCode}",
                        details,
                        Severity.Error));
                    }

                    return default;
                }
            }

            using var stream = response.Content.ReadAsStreamAsync().Result;
            return Deserialize<T>(stream);
        }
    }
}