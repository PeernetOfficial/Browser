using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public abstract class FileParameterModel
    {
        public List<FileModel> FileModels { get; set; }

        public abstract string ModalTitle { get; }

        public abstract bool ShouldUpdateFormat { get; }

        public abstract Task Confirm(FileModel[] files);
    }
}