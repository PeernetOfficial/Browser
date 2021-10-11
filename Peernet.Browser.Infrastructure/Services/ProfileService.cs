using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Models.Presentation.Profile;
using System.Threading.Tasks;

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
            var image = await profileClient.GetUserImage();
            var name = await profileClient.GetUserName();

            return new User
            {
                Name = string.IsNullOrEmpty(name) ? null : name,
                Image = image?.Length == 0 ? null : image
            };
        }

        public async Task UpdateUser(string name, byte[] image)
        {
            await profileClient.AddUserName(name);
            await profileClient.AddUserImage(image);
        }

        public async Task DeleteUserImage()
        {
            await profileClient.DeleteUserImage();
        }
    }
}