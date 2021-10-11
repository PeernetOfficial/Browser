using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Models.Presentation;
using System.Collections.Generic;
using Peernet.Browser.Models.Presentation.Profile;

namespace Peernet.Browser.Application.Contexts
{
    public interface IUserContext
    {
        bool HasUserChanged { get; }

        List<MenuItemViewModel> Items { get; }

        User User { get; set; }

        void ReloadContext();
    }
}