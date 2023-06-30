using Peernet.SDK.Models.Presentation.Profile;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public interface IUserContext
    {
        event PropertyChangedEventHandler PropertyChanged;

        string PeerId { get; set; }

        string NodeId { get; set; }

        User User { get; set; }

        void ReloadContext();
    }
}