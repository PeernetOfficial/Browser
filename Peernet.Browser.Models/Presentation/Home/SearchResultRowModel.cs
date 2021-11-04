﻿using Peernet.Browser.Models.Domain.Common;
using System;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class SearchResultRowModel
    {
        private bool isHovered;

        public SearchResultRowModel(ApiFile source)
        {
            File = source;
            EnumerationMember = (HealthType)3;
            Name = source.Name;
            Date = source.Date.ToString("dd.MM.yyyy");
            Size = $"{source.Size} MB";
            SharedBy = source.SharedByCount;
            //FlameIsVisible = source.SharedByCount > 15;
            Points = new[] { new GeoPoint { Longitude = 19, Latitude = 49 }, new GeoPoint { Longitude = 0, Latitude = 0 } };
        }

        public string Date { get; }
        public HealthType EnumerationMember { get; }

        public ApiFile File { get; }
        public bool FlameIsVisible { get; }
        public bool IsCompleted { get; }

        public bool IsHovered
        {
            get => isHovered;
            set
            {
                isHovered = value;
                if (IsHovered) OnHover?.Invoke(Points);
            }
        }

        public string Name { get; }
        public Action<GeoPoint[]> OnHover { get; set; }
        public GeoPoint[] Points { get; }
        public int SharedBy { get; }
        public string Size { get; }

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