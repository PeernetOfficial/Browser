using System.Collections.Generic;
using System.ComponentModel;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystemEntity : MvxNotifyPropertyChanged
    {
        private bool isVisualTreeVertex;

        protected VirtualFileSystemEntity(string name, VirtualFileSystemEntityType type, List<ApiBlockRecordFile> files)
        {
            Name = name;
            Type = type;
            Files = files;
        }

        public string Name { get; }

        public VirtualFileSystemEntityType Type { get; }

        public List<ApiBlockRecordFile> Files { get; }

        public bool IsVisualTreeVertex
        {
            get => isVisualTreeVertex;
            set => SetProperty(ref isVisualTreeVertex, value);
        }

        public virtual List<ApiBlockRecordFile> GetAllFiles() => Files;
    }
}