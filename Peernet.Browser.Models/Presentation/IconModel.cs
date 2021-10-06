using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Models.Presentation
{
    public class IconModel : MvxNotifyPropertyChanged
    {
        private readonly bool showCount;
        public FiltersType FilterType { get; }

        public IconModel(FiltersType filterType, bool alwaysSelected = false, Action<IconModel> onClick = null, int? count = null)
        {
            showCount = count.HasValue;
            FilterType = filterType;
            Count = count.GetValueOrDefault();
            RefreshName();
            if (alwaysSelected)
            {
                IsSelected = true;
                ShowArrow = true;
            }
            SelectCommand = new MvxCommand(() =>
            {
                if (!alwaysSelected) IsSelected = !IsSelected;
                onClick?.Invoke(this);
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

        public IMvxCommand SelectCommand { get; }
    }
}