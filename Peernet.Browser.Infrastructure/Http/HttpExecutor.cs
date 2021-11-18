﻿using Newtonsoft.Json;
using Peernet.Browser.Application.Managers;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Http
{
    internal class HttpExecutor : IHttpExecutor
    {
        private readonly HttpClient httpClient;

        public HttpExecutor(ISettingsManager settingsManager)
        {
            httpClient = new HttpClientFactory(settingsManager).CreateHttpClient();
        }

        public async Task<T> GetResultAsync<T>(
            HttpMethod method,
            string relativePath,
            Dictionary<string, string> queryParameters = null,
            HttpContent content = null)
        {
            var httpRequestMessage = PrepareMessage(relativePath, method, queryParameters, content);
            var response = await httpClient.SendAsync(httpRequestMessage);

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

        public T GetResult<T>(HttpMethod method, string relativePath, Dictionary<string, string> queryParameters = null, HttpContent content = null)
        {
            var httpRequestMessage = PrepareMessage(relativePath, method, queryParameters, content);
            var response = httpClient.Send(httpRequestMessage);

            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent &&
                response.StatusCode != HttpStatusCode.Created)
            {
                throw new HttpRequestException($"Unexpected response status code: {response.StatusCode}");
            }

            using var stream = response.Content.ReadAsStreamAsync().Result;
            return Deserialize<T>(stream);
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

        private static T Deserialize<T>(Stream stream)
        {
            using var textReader = new StreamReader(stream);
            using var reader = new JsonTextReader(textReader);
            return new JsonSerializer().Deserialize<T>(reader);
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
    }
}