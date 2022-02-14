using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Profile;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ProfileClient : ClientBase, IProfileClient
    {
        private const string DeleteSegment = "delete";
        private const string WriteSegment = "write";

        private readonly IHttpExecutor httpExecutor;

        public ProfileClient(IHttpExecutor httpExecutor)
        {
            this.httpExecutor = httpExecutor;
        }

        public override string CoreSegment => "profile";

        public async Task<ApiBlockchainBlockStatus> UpdateUser(string userName, byte[] image)
        {
            var jsonContent = JsonContent.Create(new ApiProfileData
            {
                Fields = new List<ApiBlockRecordProfile>
                {
                    new()
                    {
                        Type = ProfileField.ProfileFieldName,
                        Text = userName
                    },
                    new()
                    {
                        Type = ProfileField.ProfilePicture,
                        Blob = image
                    }
                }
            });

            return await httpExecutor.GetResultAsync<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath(WriteSegment), content: jsonContent);
        }

        public async Task<ApiBlockchainBlockStatus> DeleteUserImage()
        {
            var jsonContent = JsonContent.Create(new ApiProfileData
            {
                Fields = new List<ApiBlockRecordProfile>
                {
                    new()
                    {
                        Type = ProfileField.ProfilePicture,
                    }
                }
            });

            return await httpExecutor.GetResultAsync<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath(DeleteSegment), content: jsonContent);
        }

        public async Task<ApiProfileData> GetProfileData()
        {
            var result = await httpExecutor.GetResultAsync<ApiProfileData>(HttpMethod.Get, GetRelativeRequestPath("list"));

            return result;
        }
    }
}