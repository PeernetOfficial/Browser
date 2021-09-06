using Peernet.Browser.Application.Models;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IProfileService
    {
        string GetUserName();

        byte[] GetUserImage();

        ApiBlockchainBlockStatus AddUserName(string userName);

        ApiBlockchainBlockStatus AddUserImage(string imagePath);
    }
}