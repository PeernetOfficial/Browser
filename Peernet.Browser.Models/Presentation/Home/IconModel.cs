using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class IconModel : MvxNotifyPropertyChanged
    {
        private readonly bool showCount;

        private bool isSelected;

        public IconModel(FiltersType filterType, bool showArrow = false, Func<IconModel, Task> onClick = null, int? count = null)
        {
            showCount = count.HasValue;
            FilterType = filterType;
            Count = count.GetValueOrDefault();
            RefreshName();
            ShowArrow = showArrow;
            SelectCommand = new MvxAsyncCommand(async () =>
            {
                IsSelected ^= true;
                if (onClick != null)
                {
                    await onClick.Invoke(this);
                }
            });
        }

        public int Count { get; }

        public FiltersType FilterType { get; }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public string Name { get; private set; }

        public IMvxAsyncCommand SelectCommand { get; }

        public bool ShowArrow { get; }

        private void RefreshName()
        {
            var suffix = showCount ? $" ({Count})" : "";
            Name = $"{FilterType}{suffix}";
            RaisePropertyChanged(nameof(Name));
        }
    }
}