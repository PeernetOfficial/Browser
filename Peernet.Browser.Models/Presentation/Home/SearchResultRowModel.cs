using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchResultRowModel
    {
        public SearchResultRowModel(ApiBlockRecordFile source)
        {
            Source = source;
            EnumerationMember = (HealthType)3;
            Name = source.Name;
            Date = source.Date.ToString("dd.MM.yyyy");
            Size = $"{source.Size} MB";
            SharedBy = source.SharedByCount;
            //FlameIsVisible = source.SharedByCount > 15;
        }

        public HealthType EnumerationMember { get; }

        public string Name { get; }
        public string Date { get; }
        public string Size { get; }
        public int SharedBy { get; }
        public bool FlameIsVisible { get; }

        public ApiBlockRecordFile Source { get; }

        public static DataGridSortingNameEnum Parse(string name)
        {
            switch (name)
            {
                case nameof(Name):
                    return DataGridSortingNameEnum.Name;

                case nameof(Date):
                    return DataGridSortingNameEnum.Date;

                case nameof(Size):
                    return DataGridSortingNameEnum.Size;

                case nameof(SharedBy):
                    return DataGridSortingNameEnum.Share;

                default:
                    return DataGridSortingNameEnum.None;
            }
        }
    }
}