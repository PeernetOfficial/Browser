using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace Peernet.Browser.Application.ViewModels
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

        private string _modyfieDate;

        public string ModyfieDate
        {
            get => _modyfieDate;
            set => SetProperty(ref _modyfieDate, value);
        }
    }

    public class ModalViewModel : MvxViewModel
    {
        public ModalViewModel()
        {
            ConfirmCommand = new MvxCommand(Confirm);
            CreateDate = "Wednesday, 18 August 2021 at 16:05";
        }

        private string _createDate;

        public string CreateDate
        {
            get => _createDate;
            set => SetProperty(ref _createDate, value);
        }

        public MvxObservableCollection<SharedFileModel> Files { get; set; }

        private void Confirm()
        {
        }

        public IMvxCommand ConfirmCommand { get; }
    }
}