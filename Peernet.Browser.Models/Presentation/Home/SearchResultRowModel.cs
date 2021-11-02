using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchResultRowModel
    {
        public SearchResultRowModel(ApiFile source)
        {
            File = source;
            EnumerationMember = (HealthType)3;
            Name = source.Name;
            Date = source.Date.ToString("dd.MM.yyyy");
            Size = $"{source.Size} MB";
            SharedBy = source.SharedByCount;
            //FlameIsVisible = source.SharedByCount > 15;
            Points = new GeoPoint[0];
        }

        public HealthType EnumerationMember { get; }

        public bool IsCompleted { get; }

        public string Name { get; }
        public string Date { get; }
        public string Size { get; }
        public int SharedBy { get; }
        public bool FlameIsVisible { get; }

        public GeoPoint[] Points { get; }

        public ApiFile File { get; }

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