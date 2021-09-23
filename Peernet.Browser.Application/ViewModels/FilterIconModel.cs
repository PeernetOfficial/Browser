using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;

namespace Peernet.Browser.Application.ViewModels
{
    public class FilterIconModel : MvxNotifyPropertyChanged
    {
        public Filters FilterType { get; }

        public FilterIconModel(int count, Filters filterType, Action<FilterIconModel> selected)
        {
            FilterType = filterType;
            Count = count;
            SelectCommand = new MvxCommand(() => selected(this));
            Name = GetName();
            ImgSource = GetSource();
        }

        public int Count { get; }

        public string ImgSource { get; private set; }

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (SetProperty(ref isSelected, value))
                {
                    ImgSource = GetSource();
                    RaisePropertyChanged(nameof(ImgSource));
                }
            }
        }

        public string Name { get; }

        public IMvxCommand SelectCommand { get; }

        private string GetSource() => $"/Assets/Filters/{FilterType}{(isSelected ? "_active" : "")}.svg";

        private string GetName() => $"{FilterType} ({Count})";
    }
}