using MvvmCross.ViewModels;
using System;
using System.IO;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class SharedNewFileModel : MvxNotifyPropertyChanged
    {
        private string author;
        private string createDate;
        private string desc;
        private string directory;
        private string fileName;
        private string fileExtension;
        private string fullPath;
        private string size;
        private byte[] hash;
        private string baseName;

        public SharedNewFileModel(string path, string userName)
        {
            var f = new FileInfo(path);
            FileExtension = f.Extension;
            FullPath = f.FullName;
            BaseName = f.FullName.Replace(f.Extension, string.Empty);
            FileName = f.Name;
            Size = GetSizeString(f.Length);
            Author = userName;
            CreateDate = DateTime.Now.ToString();
            Directory = "Root";
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

        public string BaseName
        {
            get => baseName;
            set => SetProperty(ref baseName, value);
        }

        public string FileExtension
        {
            get => fileExtension;
            set => SetProperty(ref fileExtension, value);
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

        public byte[] Hash
        {
            get => hash;
            set => SetProperty(ref hash, value);
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