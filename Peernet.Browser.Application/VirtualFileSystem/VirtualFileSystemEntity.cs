using System.Collections.Generic;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemEntity : MvxNotifyPropertyChanged
    {
        private bool isVisualTreeVertex;
        private bool isSelected;

        protected VirtualFileSystemEntity(string name, VirtualFileSystemEntityType type, List<ApiBlockRecordFile> files)
        {
            Name = name;
            Type = type;
            Files = files;
        }

        public string Name { get; }

        public VirtualFileSystemEntityType Type { get; }

        public List<ApiBlockRecordFile> Files { get; }

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

        public virtual List<ApiBlockRecordFile> GetAllFiles() => Files;
    }
}