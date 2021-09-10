using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.Services;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure
{
    public class ProfileService : ServiceBase, IProfileService
    {
        private const string ReadSegment = "read";
        private const string WriteSegment = "write";

        public ProfileService(IRestClientFactory restClientFactory)
            : base(restClientFactory)
        {
        }

        public override string CoreSegment => "profile";

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

            return Task.Run(async () => await RestClient.PostAsync<ApiBlockchainBlockStatus>(request)).GetResultBlockingWithoutContextSynchronization();
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

            return Task.Run(async () => await RestClient.PostAsync<ApiBlockchainBlockStatus>(request)).GetResultBlockingWithoutContextSynchronization();
        }

        public byte[] GetUserImage()
        {
            int userImageBlobIndex = (int)ProfileBlob.ProfileBlobPicture;

            var request = new RestRequest(GetRelativeRequestPath(ReadSegment), Method.GET);
            request.AddParameter("blob", userImageBlobIndex);
            request.RequestFormat = DataFormat.Json;

            var response = Task.Run(async () =>
            {
                return await RestClient.GetAsync<ApiProfileData>(request);
            }).GetResultBlockingWithoutContextSynchronization();

            return response.Blobs?.FirstOrDefault(f => f.Type == userImageBlobIndex)?.Data;
        }

        public string GetUserName()
        {
            int userNameFieldIndex = (int)ProfileField.ProfileFieldName;

            var request = new RestRequest(GetRelativeRequestPath(ReadSegment), Method.GET);
            request.AddParameter("field", userNameFieldIndex);

            var response = Task.Run(async () => await RestClient.GetAsync<ApiProfileData>(request)).GetResultBlockingWithoutContextSynchronization();

            return response.Fields?.FirstOrDefault(f => f.Type == userNameFieldIndex)?.Text;
        }
    }
}