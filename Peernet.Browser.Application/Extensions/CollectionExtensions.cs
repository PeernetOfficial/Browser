using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Plugins;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Extensions
{
    public static class CollectionExtensions
    {
        public static void SetPlayerState(this IEnumerable<DownloadModel> results, IEnumerable<IPlayButtonPlug> playButtonPlugs)
        {
            results.Foreach(r =>
            {
                r.IsPlayerEnabled = playButtonPlugs.Any(plug => plug?.IsSupported(r.File) == true);
            });
        }
    }
}