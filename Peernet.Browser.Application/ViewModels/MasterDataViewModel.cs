using Peernet.SDK.Models.Presentation.Profile;

namespace Peernet.Browser.Application.ViewModels
{
    public class MasterDataViewModel
    {
        public MasterDataViewModel(User user)
        {
            User = user;
        }

        public User User { get; set; }
    }
}