using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public abstract class FileParameterModel
    {
        public FileModel[] FileModels { get; set; }

        public abstract Task Confirm(FileModel[] files);
    }
}