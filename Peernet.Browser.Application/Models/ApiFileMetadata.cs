using System;

namespace Peernet.Browser.Application.Models
{
    public class ApiFileMetadata
    {
        public MetadataType Type { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public byte[] Blob { get; set; }

        public DateTime Date { get; set; }

        public int Number { get; set; }
    }
}
