using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Threading.Tasks;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class IconModel : MvxNotifyPropertyChanged
    {
        private readonly bool showCount;
        public FiltersType FilterType { get; }

        public IconModel(FiltersType filterType, bool showArrow = false, Func<IconModel, Task> onClick = null, int? count = null)
        {
            showCount = count.HasValue;
            FilterType = filterType;
            Count = count.GetValueOrDefault();
            RefreshName();
            ShowArrow = showArrow;
            SelectCommand = new MvxAsyncCommand(async () =>
            {
                IsSelected = !IsSelected;
                await onClick?.Invoke(this);
            });
        }

        public int Count { get; }

        public bool ShowArrow { get; }

        private void RefreshName()
        {
            var surfix = showCount ? $" ({Count})" : "";
            Name = $"{FilterType}{surfix}";
            RaisePropertyChanged(nameof(Name));
        }

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public string Name { get; private set; }

        public IMvxAsyncCommand SelectCommand { get; }
    }
}