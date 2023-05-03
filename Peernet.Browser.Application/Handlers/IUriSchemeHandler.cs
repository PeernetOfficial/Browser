using System.Threading.Tasks;

namespace Peernet.Browser.Application.Handlers
{
    public interface IUriSchemeHandler
    {
        Task Handle(string url);
    }
}