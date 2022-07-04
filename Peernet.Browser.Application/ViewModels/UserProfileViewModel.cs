using Peernet.Browser.Application.Download;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Profile;
using System.Collections.Generic;

namespace Peernet.Browser.Application.ViewModels
{
    public class UserProfileViewModel
    {
        public UserProfileViewModel(User user, IDownloadManager downloadManager, IEnumerable<IPlayButtonPlug> plugs)
        {
            MasterData = new(user);
            SharedData = new(user, downloadManager, plugs);
        }

        public MasterDataViewModel MasterData { get; set; }

        public SharedDataViewModel SharedData { get; set; }
    }
}