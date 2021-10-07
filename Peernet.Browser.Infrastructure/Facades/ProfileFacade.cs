using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Wrappers;
using Peernet.Browser.Models.Presentation;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class ProfileFacade : IProfileFacade
    {
        private readonly IProfileWrapper profileService;

        public ProfileFacade(ISettingsManager settingsManager)
        {
            this.profileService = new ProfileWrapper(settingsManager);
        }

        public async Task<User> GetUser()
        {
            var image = await profileService.GetUserImage();
            var name = await profileService.GetUserName();

            return new User
            {
                Name = string.IsNullOrEmpty(name) ? null : name,
                Image = image?.Length == 0 ? null : image
            };
        }

        public async Task UpdateUser(string name, byte[] image)
        {
            await profileService.AddUserName(name);
            await profileService.AddUserImage(image);
        }

        public async Task DeleteUserImage()
        {
            await profileService.DeleteUserImage();
        }
    }
}