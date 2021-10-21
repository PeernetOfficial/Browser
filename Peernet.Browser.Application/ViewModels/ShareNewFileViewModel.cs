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
    public class ShareNewFileViewModel : MvxViewModel<string[]>
    {
        private readonly IApplicationManager applicationManager;
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IBlockchainService blockchainService;
        private readonly IWarehouseService warehouseService;
        private readonly IUserContext userContext;
        private SharedNewFileModel selected;

        public ShareNewFileViewModel(IMvxNavigationService mvxNavigationService, IApplicationManager applicationManager, IBlockchainService blockchainService, IWarehouseService warehouseService, IUserContext userContext)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.applicationManager = applicationManager;
            this.blockchainService = blockchainService;
            this.warehouseService = warehouseService;
            this.userContext = userContext;

            ConfirmCommand = new MvxAsyncCommand(Confirm);
            CancelCommand = new MvxCommand(Cancel);

            LeftCommand = new MvxCommand(() => Manipulate(false));
            RightCommand = new MvxCommand(() => Manipulate(true));

            AddCommand = new MvxCommand(Add);
            DeleteFileCommand = new MvxCommand(DeleteFile);

            Files.CollectionChanged += (s, o) =>
            {
                RaisePropertyChanged(nameof(FilesLength));
                RaisePropertyChanged(nameof(IsCountVisible));
            };
        }

        public IMvxCommand AddCommand { get; }

        public IMvxAsyncCommand ConfirmCommand { get; }

        public MvxObservableCollection<SharedNewFileModel> Files { get; } = new MvxObservableCollection<SharedNewFileModel>();

        public string FilesLength => $"{Files.IndexOf(Selected) + 1}/{Files.Count}";

        public IMvxCommand CancelCommand { get; }

        public bool IsCountVisible => Files.Count > 1;

        public IMvxCommand LeftCommand { get; }

        public IMvxCommand RightCommand { get; }

        public IMvxCommand DeleteFileCommand { get; }

        public SharedNewFileModel Selected
        {
            get => selected;
            private set => SetProperty(ref selected, value);
        }

        public override void Prepare(string[] files)
        {
            foreach (var f in files)
            {
                var toAdd = new SharedNewFileModel(f, userContext.User.Name);
                if (Files.Any(x => x.FullPath == toAdd.FullPath))
                {
                    continue;
                }

                Files.Add(toAdd);
            }
            Selected = Files.First();
        }

        private void Add()
        {
            var files = applicationManager.OpenFileDialog();
            if (files.Length != 0)
            {
                Prepare(files);
            }
        }

        private async Task Confirm()
        {
            // There should be validation added all the way within this method
            foreach (var file in Files)
            {
                var warehouseEntry = await warehouseService.Create(file);
                if (warehouseEntry.Status == 0)
                {
                    file.Hash = warehouseEntry.Hash;
                }
            }

            await blockchainService.AddFiles(Files.Where(f => f.Hash != null));
            Cancel();
        }

        private void Cancel()
        {
            GlobalContext.IsMainWindowActive = true;
            mvxNavigationService.Close(this);
        }

        private void Manipulate(bool isPlus)
        {
            var i = isPlus ? 1 : -1;
            var index = Files.IndexOf(Selected) + i;
            if (index >= 0 && index <= Files.Count - 1)
            {
                Selected = Files[index];
                RaisePropertyChanged(nameof(FilesLength));
            }
        }

        private void DeleteFile()
        {
            var removedIndex = Files.IndexOf(Selected);
            Files.Remove(Selected);
            var filesCount = Files.Count;
            if (filesCount == 0)
            {
                Cancel();
            }
            else
            {
                if (removedIndex == 0)
                {
                    Selected = Files[0];
                }
                else if (removedIndex > 0)
                {
                    Selected = Files.ElementAt(filesCount - 1);
                }

                RaisePropertyChanged(nameof(FilesLength));
            }
        }
    }
}