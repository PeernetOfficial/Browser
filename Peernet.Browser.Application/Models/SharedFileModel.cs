using MvvmCross.ViewModels;
using System;
using System.IO;

namespace Peernet.Browser.Application.Models
{
    public class SharedNewFileModel : MvxNotifyPropertyChanged
    {
        private string author;

        private string createDate;

        private string desc;

        private string directory;

        private string fileName;

        private string fileType;

        private string fullPath;

        private string size;

        public SharedNewFileModel(string path)
        {
            var f = new FileInfo(path);
            FileType = f.Extension;
            FullPath = f.Name.Replace(FileType, "");
            FileName = FullPath;
            Size = GetSizeString(f.Length);
            Author = "Current user";
            CreateDate = DateTime.Now.ToString();
            Directory = "Default";
        }

        public string Author
        {
            get => author;
            set => SetProperty(ref author, value);
        }

        public string CreateDate
        {
            get => createDate;
            set => SetProperty(ref createDate, value);
        }

        public string Desc
        {
            get => desc;
            set => SetProperty(ref desc, value);
        }

        public string Directory
        {
            get => directory;
            set => SetProperty(ref directory, value);
        }

        public string FileName
        {
            get => fileName;
            set => SetProperty(ref fileName, value);
        }

        public string FileType
        {
            get => fileType;
            set => SetProperty(ref fileType, value);
        }

        public string FullPath
        {
            get => fullPath;
            set => SetProperty(ref fullPath, value);
        }

        public string Size
        {
            get => size;
            set => SetProperty(ref size, value);
        }

        private string GetSizeString(long o)
        {
            var len = o;
            var sizes = new[] { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{o} bytes ({len:0.##} {sizes[order]})";
        }
    }
}