using Newtonsoft.Json;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Application.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public class ProfileService : IProfileService
    {
        private RestClient restClient;
        private const string CoreSegment = "profile";
        private const string ReadSegment = "read";
        private const string WriteSegment = "write";

        public ProfileService(IRestClientFactory restClientFactory)
        {
            restClient = restClientFactory.CreateRestClient();
        }

        public string GetUserName()
        {
            int userNameFieldIndex = (int)ProfileField.ProfileFieldName;

            var request = new RestRequest(GetRelativeRequestPath(ReadSegment), Method.GET);
            request.AddParameter("field", userNameFieldIndex);

            var response = Task.Run(async () => await restClient.GetAsync<ApiProfileData>(request)).ConfigureAwait(false).GetAwaiter().GetResult();

            return response.Fields?.FirstOrDefault(f => f.Type == userNameFieldIndex)?.Text;
        }

        public byte[] GetUserImage()
        {
            int userImageBlobIndex = (int)ProfileBlob.ProfileBlobPicture;

            var request = new RestRequest(GetRelativeRequestPath(ReadSegment), Method.GET);
            request.AddParameter("blob", userImageBlobIndex);
            request.RequestFormat = DataFormat.Json;

            var response = Task.Run(async () =>
            {
                var response = await restClient.ExecuteAsync(request);

                // Deserialization to byte[] is not supported by System.Text.Json.JsonSerializer as well as RestSharp.Deserializers.JsonDeserializer linked to ResClient
                return JsonConvert.DeserializeObject<ApiProfileData>(response.Content);
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            return response.Blobs?.FirstOrDefault(f => f.Type == userImageBlobIndex)?.Data;
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

        public ApiBlockchainBlockStatus AddUserImage(string imagePath)
        {
            var request = new RestRequest(GetRelativeRequestPath(WriteSegment), Method.POST);
            request.AddJsonBody(new ApiProfileData
            {
                Blobs = new List<Blob>
                {
                    new Blob
                    {
                        Type = 0,
                        Data = ConvertImageToByteArray(Image.FromFile(imagePath))
                    }
                }
            });

            return Task.Run(async () => await restClient.PostAsync<ApiBlockchainBlockStatus>(request)).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private Uri GetRelativeRequestPath(string readWriteSegment)
        {
            return new Uri($"{CoreSegment}/{readWriteSegment}", UriKind.Relative);
        }

        public byte[] ConvertImageToByteArray(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            var array = ms.ToArray();

            return array;
        }
    }
}