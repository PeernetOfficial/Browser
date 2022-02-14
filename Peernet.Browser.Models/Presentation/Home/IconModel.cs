using AsyncAwaitBestPractices.MVVM;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class IconModel : INotifyPropertyChanged
    {
        private readonly bool showCount;

        private bool isSelected;

        public IconModel(FilterType filterType, bool showArrow = false, Func<IconModel, Task> onClick = null, int? count = null)
        {
            showCount = count.HasValue;
            FilterType = filterType;
            Count = count.GetValueOrDefault();
            RefreshName();
            ShowArrow = showArrow;
            SelectCommand = new AsyncCommand(async () =>
            {
                IsSelected ^= true;
                if (onClick != null)
                {
                    await onClick.Invoke(this);
                }
            });
        }

        public int Count { get; }

        public FilterType FilterType { get; }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new(nameof(IsSelected)));
            }
        }

        public string Name { get; private set; }

        public IAsyncCommand SelectCommand { get; }

        public bool ShowArrow { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RefreshName()
        {
            var suffix = showCount ? $" ({Count})" : "";
            Name = $"{FilterType}{suffix}";
            PropertyChanged?.Invoke(this, new(nameof(Name)));
        }
    }
}