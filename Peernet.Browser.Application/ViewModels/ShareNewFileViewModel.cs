using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Models.Presentation.Footer;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class ShareNewFileViewModel : MvxViewModel<string[]>, IModal
    {
        private readonly IApplicationManager applicationManager;
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IBlockchainService blockchainService;
        private SharedNewFileModel selected;

        public ShareNewFileViewModel(IMvxNavigationService mvxNavigationService, IApplicationManager applicationManager, IBlockchainService blockchainService)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.applicationManager = applicationManager;
            this.blockchainService = blockchainService;

            ConfirmCommand = new MvxAsyncCommand(Confirm);
            HideCommand = new MvxCommand(Hide);

            LeftCommand = new MvxCommand(() => Manipulate(false));
            RightCommand = new MvxCommand(() => Manipulate(true));

            ChangeCommand = new MvxCommand(Change);
            AddCommand = new MvxCommand(Add);

            Files.CollectionChanged += (s, o) =>
            {
                RaisePropertyChanged(nameof(FilesLength));
                RaisePropertyChanged(nameof(IsCountVisible));
            };
        }

        public IMvxCommand AddCommand { get; }

        public IMvxCommand ChangeCommand { get; }

        public IMvxAsyncCommand ConfirmCommand { get; }

        public MvxObservableCollection<SharedNewFileModel> Files { get; } = new MvxObservableCollection<SharedNewFileModel>();

        public string FilesLength => $"{Files.IndexOf(Selected) + 1}/{Files.Count}";

        public IMvxCommand HideCommand { get; }

        public bool IsCountVisible => Files.Count > 1;

        public IMvxCommand LeftCommand { get; }

        public IMvxCommand RightCommand { get; }

        public SharedNewFileModel Selected
        {
            get => selected;
            private set => SetProperty(ref selected, value);
        }

        public override void Prepare(string[] files)
        {
            foreach (var f in files)
            {
                var toAdd = new SharedNewFileModel(f);
                if (Files.Any(x => x.FullPath == toAdd.FullPath)) continue;
                Files.Add(toAdd);
            }
            Selected = Files.First();
        }

        private void Add()
        {
            var files = applicationManager.OpenFileDialog();
            if (files.Any()) Prepare(files);
        }

        private void Change()
        {
            //TODO: USE service??
        }

        private async Task Confirm()
        {
            await blockchainService.AddFilesAsync(Files);
            Hide();
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
    }
}