using Newtonsoft.Json;

namespace Peernet.Browser.Application.Utilities
{
    public static class MessagingHelper
    {
        public static string GetInOutSummary(object input, object output)
        {
            return $"Input: {JsonConvert.SerializeObject(input)}\nOutput: {JsonConvert.SerializeObject(output)}";
        }
    }
}
