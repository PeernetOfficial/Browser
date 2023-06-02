using Peernet.SDK.Models.Presentation.Profile;

namespace Peernet.Browser.Application.Contexts
{
    public interface IUserContext
    {
        bool HasUserChanged { get; set; }

        string PeerId { get; set; }

        string NodeId { get; set; }

        User User { get; set; }

        void ReloadContext();
    }
}