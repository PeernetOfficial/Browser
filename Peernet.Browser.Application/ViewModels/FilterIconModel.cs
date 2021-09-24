using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using Peernet.Browser.Application.VirtualFileSystem;

namespace Peernet.Browser.Application.ViewModels
{
    public class FilterIconModel : MvxNotifyPropertyChanged
    {
        public VirtualFileSystemEntityType Type { get; }

        public FilterIconModel(int count, VirtualFileSystemEntityType type, Action<FilterIconModel> selected)
        {
            Type = type;
            Count = count;
            SelectCommand = new MvxCommand(() => selected(this));
            Name = GetName();
        }

        public int Count { get; }

        private bool isSelected;

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public string Name { get; }

        public IMvxCommand SelectCommand { get; }

        private string GetName() => $"{Type} ({Count})";
    }
}