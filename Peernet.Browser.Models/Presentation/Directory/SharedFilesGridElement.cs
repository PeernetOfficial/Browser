using Peernet.Browser.Models.Domain.Common;
using System;

namespace Peernet.Browser.Models.Presentation.Directory
{
    public class SharedFilesGridElement
    {
        public string Name { get; set; }

        public LowLevelFileType Type { get; set; }

        public DateTime Date { get; set; }
    }
}