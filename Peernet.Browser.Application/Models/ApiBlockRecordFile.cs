using System;
using System.Collections.Generic;

namespace Peernet.Browser.Application.Models
{
    public class ApiBlockRecordFile
    {
        public string Id { get; set; }

        public byte[] Hash { get; set; }

        public LowLevelFileType Type { get; set; }

        public HighLevelFileType Format { get; set; }

        public int Size { get; set; }

        public string Folder { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public List<ApiFileMetadata> MetaData { get; set; }

        public List<ApiFileTagRaw> TagsRaw { get; set; }
    }
}