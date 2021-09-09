using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.ViewModels
{
    public class ModalViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mvxNavigationService;

        private SharedFileModel selected;

        public SharedFileModel Selected
        {
            get => selected;
            private set => SetProperty(ref selected, value);
        }

        public bool IsCountVisable => Files.Count > 1;

        public string FilesLength => $"{Files.IndexOf(Selected) + 1}/{Files.Count}";

        public ModalViewModel(IMvxNavigationService mvxNavigationService)
        {
            this.mvxNavigationService = mvxNavigationService;

            ConfirmCommand = new MvxCommand(Confirm);
            HideCommand = new MvxCommand(Hide);

            LeftCommand = new MvxCommand(() => Manipulate(false));
            RightCommand = new MvxCommand(() => Manipulate(true));

            ChangeCommand = new MvxCommand(Change);
            AddCommand = new MvxCommand(Add);

            Files.CollectionChanged += (s, o) =>
            {
                RaisePropertyChanged(nameof(FilesLength));
                RaisePropertyChanged(nameof(IsCountVisable));
            };
        }

        public MvxObservableCollection<SharedFileModel> Files { get; } = new MvxObservableCollection<SharedFileModel>();

        private void Confirm()
        {
            Hide();
        }

        private void Add()
        {
            Selected = new SharedFileModel
            {
                FullPath = "Permanent Record_book_by_Edward_Snowden.",
                FileName = $"Permanent Record {Files.Count + 1}",
                FileType = "pdf",
                Size = "350 718 bytes (352 KB)",
                CreateDate = "Wednesday, 18 August 2021 at 16:05",
                Author = "ElonMusk2",
                ModyfieDate = "Wednesday, 18 August 2021 at 16:05"
            };
            Files.Add(Selected);
        }

        private void Hide()
        {
            GlobalContext.IsMainWindowActive = true;
            mvxNavigationService.Close(this);
        }

        private void Manipulate(bool isPlus)
        {
            var i = isPlus ? 1 : -1;
            var index = Files.IndexOf(Selected) + i;
            if (index < 0 || index > Files.Count - 1) return;
            Selected = Files[index];
            RaisePropertyChanged(nameof(FilesLength));
        }

        private void Change()
        {
        }

        public IMvxCommand ConfirmCommand { get; }
        public IMvxCommand HideCommand { get; }
        public IMvxCommand LeftCommand { get; }
        public IMvxCommand RightCommand { get; }
        public IMvxCommand AddCommand { get; }
        public IMvxCommand ChangeCommand { get; }
    }
}