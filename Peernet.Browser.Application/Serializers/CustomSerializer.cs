using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;
using System.IO;

namespace Peernet.Browser.Application.Serializers
{
    class CustomSerializer : IDeserializer
    {
        public T Deserialize<T>(IRestResponse response)
        {
            using var textReader = new StringReader(response.Content);
            using var reader = new JsonTextReader(textReader);

            return new JsonSerializer().Deserialize<T>(reader);
        }
    }
}
