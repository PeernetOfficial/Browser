namespace Peernet.Browser.Models.Domain.Search
{
    public class SearchRequestBase
    {
        public SearchRequestBase()
        {
            FileFormat = -1;
            FileType = -1;
            SizeMax = -1;
            SizeMin = -1;
        }

        /// <summary>
        ///  Sort order
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// File type such as binary, text document etc. See core.TypeX.
        /// </summary>
        public int FileType { get; set; }

        /// <summary>
        /// File format such as PDF, Word, Ebook, etc. See core.FormatX.
        /// </summary>
        public int FileFormat { get; set; }

        /// <summary>
        /// Min file size in bytes. -1 = not used.
        /// </summary>
        public int SizeMin { get; set; }

        /// <summary>
        /// Max file size in bytes. -1 = not used.
        /// </summary>
        public int SizeMax { get; set; }
    }
}