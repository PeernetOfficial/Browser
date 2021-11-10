using Peernet.Browser.Models.Domain.File;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.Services
{
    public interface IFileService
    {
        Task<ApiResponseFileFormat> GetFormat(string path);
    }
}