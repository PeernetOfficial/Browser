﻿using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Http;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Profile;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Clients
{
    internal class ProfileClient : ClientBase, IProfileClient
    {
        private const string DeleteSegment = "delete";
        private const string ReadSegment = "read";
        private const string WriteSegment = "write";

        private readonly IHttpExecutor httpExecutor;

        public ProfileClient(ISettingsManager settingsManager)
        {
            httpExecutor = new HttpExecutor(settingsManager);
        }

        public override string CoreSegment => "profile";

        public async Task<ApiBlockchainBlockStatus> AddUserImage(byte[] content)
        {
            var jsonContent = JsonContent.Create(new ApiProfileData
            {
                Fields = new List<ApiBlockRecordProfile>
                {
                    new()
                    {
                        Type = (int)ProfileField.ProfilePicture,
                        Blob = content
                    }
                }
            });

            return await httpExecutor.GetResult<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath(WriteSegment), content: jsonContent);
        }

        public async Task<ApiBlockchainBlockStatus> AddUserName(string userName)
        {
            var jsonContent = JsonContent.Create(new ApiProfileData
            {
                Fields = new List<ApiBlockRecordProfile>
                {
                    new()
                    {
                        Type = (int)ProfileField.ProfileFieldName,
                        Text = userName
                    }
                }
            });

            return await httpExecutor.GetResult<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath(WriteSegment), content: jsonContent);
        }

        public async Task<ApiBlockchainBlockStatus> DeleteUserImage()
        {
            var jsonContent = JsonContent.Create(new ApiProfileData
            {
                Fields = new List<ApiBlockRecordProfile>
                {
                    new()
                    {
                        Type = (int)ProfileField.ProfilePicture,
                    }
                }
            });

            return await httpExecutor.GetResult<ApiBlockchainBlockStatus>(HttpMethod.Post, GetRelativeRequestPath(DeleteSegment), content: jsonContent);
        }

        public async Task<byte[]> GetUserImage()
        {
            const int userImageBlobIndex = (int)ProfileField.ProfilePicture;

            var parameters = new Dictionary<string, string>
            {
                ["field"] = userImageBlobIndex.ToString()
            };

            var result = await httpExecutor.GetResult<ApiProfileData>(HttpMethod.Get, GetRelativeRequestPath(ReadSegment), parameters);

            return result.Fields?.FirstOrDefault(f => f.Type == userImageBlobIndex)?.Blob;
        }

        public async Task<string> GetUserName()
        {
            const int userNameFieldIndex = (int)ProfileField.ProfileFieldName;

            var parameters = new Dictionary<string, string>
            {
                ["field"] = userNameFieldIndex.ToString()
            };

            var result = await httpExecutor.GetResult<ApiProfileData>(HttpMethod.Get, GetRelativeRequestPath(ReadSegment), parameters);

            return result.Fields?.FirstOrDefault(f => f.Type == userNameFieldIndex)?.Text;
        }
    }
}