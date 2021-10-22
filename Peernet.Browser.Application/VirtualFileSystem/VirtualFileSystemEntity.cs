using MvvmCross.ViewModels;
using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemEntity : MvxNotifyPropertyChanged
    {
        private bool isVisualTreeVertex;
        private bool isSelected;

        protected VirtualFileSystemEntity(string name, VirtualFileSystemEntityType type, List<ApiFile> files)
        {
            Name = name;
            Type = type;
            Files = files;
        }

        public string Name { get; }

        public VirtualFileSystemEntityType Type { get; }

        public List<ApiFile> Files { get; }

        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        }

        public bool IsVisualTreeVertex
        {
            get => isVisualTreeVertex;
            set => SetProperty(ref isVisualTreeVertex, value);
        }

        public virtual void ResetSelection()
        {
            IsSelected = false;
        }

        public virtual List<ApiFile> GetAllFiles() => Files;
    }
}