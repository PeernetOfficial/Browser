using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class ProfileService : IProfileService
    {
        private const string CoreSegment = "profile";
        private const string ReadSegment = "read";
        private const string WriteSegment = "write";
        private readonly RestClient restClient;

        public ProfileService(IRestClientFactory restClientFactory)
        {
            restClient = restClientFactory.CreateRestClient();
        }

        public ApiBlockchainBlockStatus AddUserImage(byte[] content)
        {
            var request = new RestRequest(GetRelativeRequestPath(WriteSegment), Method.POST);
            request.AddJsonBody(new ApiProfileData
            {
                Blobs = new List<Blob>
                {
                    new Blob
                    {
                        Type = 0,
                        Data = content
                    }
                }
            });

            return Task.Run(async () => await restClient.PostAsync<ApiBlockchainBlockStatus>(request)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public ApiBlockchainBlockStatus AddUserName(string userName)
        {
            var request = new RestRequest(GetRelativeRequestPath(WriteSegment), Method.POST);
            request.AddJsonBody(new ApiProfileData
            {
                Fields = new List<Field>
                {
                    new Field
                    {
                        Type = 0,
                        Text = userName
                    }
                }
            });

            return Task.Run(async () => await restClient.PostAsync<ApiBlockchainBlockStatus>(request)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public byte[] GetUserImage()
        {
            int userImageBlobIndex = (int)ProfileBlob.ProfileBlobPicture;

            var request = new RestRequest(GetRelativeRequestPath(ReadSegment), Method.GET);
            request.AddParameter("blob", userImageBlobIndex);
            request.RequestFormat = DataFormat.Json;

            var response = Task.Run(async () =>
            {
                return await restClient.GetAsync<ApiProfileData>(request);
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            return response.Blobs?.FirstOrDefault(f => f.Type == userImageBlobIndex)?.Data;
        }

        public string GetUserName()
        {
            int userNameFieldIndex = (int)ProfileField.ProfileFieldName;

            var request = new RestRequest(GetRelativeRequestPath(ReadSegment), Method.GET);
            request.AddParameter("field", userNameFieldIndex);

            var response = Task.Run(async () => await restClient.GetAsync<ApiProfileData>(request)).ConfigureAwait(false).GetAwaiter().GetResult();

            return response.Fields?.FirstOrDefault(f => f.Type == userNameFieldIndex)?.Text;
        }

        private static Uri GetRelativeRequestPath(string readWriteSegment)
        {
            return new Uri($"{CoreSegment}/{readWriteSegment}", UriKind.Relative);
        }
    }
}