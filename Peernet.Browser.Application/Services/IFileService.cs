using Peernet.SDK.Models.Domain.File;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IFileService
    {
        Task<ApiResponseFileFormat> GetFormat(string path);
    }
}