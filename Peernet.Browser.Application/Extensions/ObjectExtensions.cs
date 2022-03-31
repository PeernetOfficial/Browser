using Newtonsoft.Json;

namespace Peernet.Browser.Application.Extensions
{
    public static class ObjectExtensions
    {
        public static T DeepClone<T>(this T source)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var serialized = JsonConvert.SerializeObject(source, settings);
            return JsonConvert.DeserializeObject<T>(serialized, settings);
        }
    }
}
