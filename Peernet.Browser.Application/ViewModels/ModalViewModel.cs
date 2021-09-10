using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Models;
using System.Linq;

namespace Peernet.Browser.Application.ViewModels
{
    public class ModalViewModel : MvxViewModel<string[]>
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

        public override void Prepare(string[] files)
        {
            foreach (var f in files) PrepareSingleFile(f);
            Selected = Files.First();
        }

        private void PrepareSingleFile(string s) => Files.Add(new SharedFileModel(s));

        public IMvxCommand ConfirmCommand { get; }
        public IMvxCommand HideCommand { get; }
        public IMvxCommand LeftCommand { get; }
        public IMvxCommand RightCommand { get; }
        public IMvxCommand AddCommand { get; }
        public IMvxCommand ChangeCommand { get; }
    }
}