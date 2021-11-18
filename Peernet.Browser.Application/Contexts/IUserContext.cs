using Peernet.Browser.Models.Presentation.Profile;

namespace Peernet.Browser.Application.Contexts
{
    public interface IUserContext
    {
        bool HasUserChanged { get; set;  }

        User User { get; set; }

        void ReloadContext();
    }
}