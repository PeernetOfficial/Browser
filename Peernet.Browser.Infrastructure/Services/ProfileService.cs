using System.Linq;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Presentation.Profile;
using System.Threading.Tasks;
using Peernet.Browser.Models.Domain.Blockchain;
using Peernet.Browser.Models.Domain.Profile;

namespace Peernet.Browser.Infrastructure.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileClient profileClient;

        public ProfileService(ISettingsManager settingsManager)
        {
            profileClient = new ProfileClient(settingsManager);
        }

        public async Task<User> GetUser()
        {
            var data = await profileClient.GetProfileData();
            var name = data.Fields?.FirstOrDefault(f => f.Type == ProfileField.ProfileFieldName)?.Text;
            var image = data.Fields?.FirstOrDefault(f => f.Type == ProfileField.ProfilePicture)?.Blob;

            return new User
            {
                Name = string.IsNullOrEmpty(name) ? null : name,
                Image = image?.Length == 0 ? null : image
            };
        }

        public async Task<ApiBlockchainBlockStatus> UpdateUser(string name, byte[] image)
        {
            return await profileClient.UpdateUser(name, image);
        }

        public async Task DeleteUserImage()
        {
            await profileClient.DeleteUserImage();
        }
    }
}