using Peernet.Browser.Models.Domain.Common;
using System;
using System.Linq;

namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchStatisticData
    {
        /// <summary>
        /// Files per date
        /// </summary>
        public SearchStatisticRecordDay[] Date { get; set; }

        /// <summary>
        /// Files per file type
        /// </summary>
        public SearchStatisticFileTypeRecord[] FileType { get; set; }

        /// <summary>
        /// Files per file format
        /// </summary>
        public SearchStatisticFileFormatRecord[] FileFormat { get; set; }

        /// <summary>
        /// Total count of files
        /// </summary>
        public int Total { get; set; }

        public int GetCount<T>(T type) where T : Enum
        {
            if (Total == 0) return 0;

            if (type is HighLevelFileType highLevelType)
            {
                var res = FileFormat.FirstOrDefault(x => x.Key == highLevelType);
                return res != null ? res.Count : 0;
            }
            else if (type is LowLevelFileType lowLevelType)
            {
                var res = FileType.FirstOrDefault(x => x.Key == lowLevelType);
                return res != null ? res.Count : 0;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}