using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Peernet.Browser.Application.Models
{
    [Serializable]
    public class ApiBlockRecordFile
    {
        public ApiBlockRecordFile()
        {
        }
        public Guid Id { get; set; }

        public byte[] Hash { get; set; }

        public int Type { get; set; }

        public int Format { get; set; }

        public int Size { get; set; }

        public string Folder { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public string Date => DateTime.Parse(MetaData.First(md => md.Name == "Date Shared").Value).ToShortDateString();

        public List<ApiFileMetadata> MetaData { get; set; }

        public List<ApiFileTagRaw> TagsRaw { get; set; }
    }
}
