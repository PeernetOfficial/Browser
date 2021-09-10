using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.Models
{
    public class SharedFileModel : MvxNotifyPropertyChanged
    {
        private string _fullPath;

        private string _fileType;

        public string FullPath
        {
            get => _fullPath;
            set => SetProperty(ref _fullPath, value);
        }

        public string FileType
        {
            get => _fileType;
            set => SetProperty(ref _fileType, value);
        }

        private string _fileName;

        public string FileName
        {
            get => _fileName;
            set => SetProperty(ref _fileName, value);
        }

        private string _desc;

        public string Desc
        {
            get => _desc;
            set => SetProperty(ref _desc, value);
        }

        private string _author;

        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private string _size;

        public string Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }

        private string _createDate;

        public string CreateDate
        {
            get => _createDate;
            set => SetProperty(ref _createDate, value);
        }

        private string _directory;

        public string Directory
        {
            get => _directory;
            set => SetProperty(ref _directory, value);
        }
    }
}