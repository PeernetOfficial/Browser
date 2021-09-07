using Peernet.Browser.Application.Models;
using Peernet.Browser.Application.ViewModels;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Contexts
{
    public interface IUserContext
    {
        List<MenuItemViewModel> Items { get; }
        User User { get; }
    }
}