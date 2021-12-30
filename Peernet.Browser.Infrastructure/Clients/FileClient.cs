using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.File;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class FileClient : ClientBase, IFileClient
    {
        private readonly IHttpExecutor httpExecutor;

        public FileClient(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => "file";

        public async Task<ApiResponseFileFormat> GetFormat(string path)
        {
            var parameters = new Dictionary<string, string>
            {
                [nameof(path)] = path
            };

            return await httpExecutor.GetResultAsync<ApiResponseFileFormat>(HttpMethod.Get, GetRelativeRequestPath("format"),
                parameters);
        }
    }
}