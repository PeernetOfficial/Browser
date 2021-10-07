using Peernet.Browser.Application.Facades;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Wrappers;
using Peernet.Browser.Models.Presentation;
using System.Threading.Tasks;

namespace Peernet.Browser.Infrastructure.Facades
{
    public class ProfileFacade : IProfileFacade
    {
        private readonly IProfileWrapper profileWrapper;

        public ProfileFacade(ISettingsManager settingsManager)
        {
            profileWrapper = new ProfileWrapper(settingsManager);
        }

        public async Task<User> GetUser()
        {
            var image = await profileWrapper.GetUserImage();
            var name = await profileWrapper.GetUserName();

            return new User
            {
                Name = string.IsNullOrEmpty(name) ? null : name,
                Image = image?.Length == 0 ? null : image
            };
        }

        public async Task UpdateUser(string name, byte[] image)
        {
            await profileWrapper.AddUserName(name);
            await profileWrapper.AddUserImage(image);
        }

        public async Task DeleteUserImage()
        {
            await profileWrapper.DeleteUserImage();
        }
    }
}