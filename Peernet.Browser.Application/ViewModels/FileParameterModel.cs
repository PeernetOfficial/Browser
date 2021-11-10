using System.Collections.Generic;
using Peernet.Browser.Models.Presentation.Footer;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public abstract class FileParameterModel
    {
        public List<FileModel> FileModels { get; set; }

        public abstract string ModalTitle { get; }

        public abstract Task Confirm(FileModel[] files);
    }
}