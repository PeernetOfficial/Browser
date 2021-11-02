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
    public class GenericFileViewModel : MvxViewModel<FileParameterModel>, IModal
    {
        private readonly IApplicationManager applicationManager;
        private readonly IMvxNavigationService mvxNavigationService;
        private readonly IBlockchainService blockchainService;
        private readonly IWarehouseService warehouseService;
        private FileModel selected;
        private FileParameterModel viewModelParameter;

        public GenericFileViewModel(IMvxNavigationService mvxNavigationService, IApplicationManager applicationManager, IBlockchainService blockchainService, IWarehouseService warehouseService)
        {
            this.mvxNavigationService = mvxNavigationService;
            this.applicationManager = applicationManager;
            this.blockchainService = blockchainService;
            this.warehouseService = warehouseService;

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

        public MvxObservableCollection<FileModel> Files { get; } = new MvxObservableCollection<FileModel>();

        public string FilesLength => $"{Files.IndexOf(Selected) + 1}/{Files.Count}";

        public IMvxCommand CancelCommand { get; }

        public bool IsCountVisible => Files.Count > 1;

        public IMvxCommand LeftCommand { get; }

        public IMvxCommand RightCommand { get; }

        public IMvxCommand DeleteFileCommand { get; }

        public FileModel Selected
        {
            get => selected;
            private set => SetProperty(ref selected, value);
        }

        public string Title => viewModelParameter.ModalTitle;

        public override void Prepare(FileParameterModel parameter)
        {
            viewModelParameter = parameter;
            foreach (var f in parameter.FileModels)
            {
                if (Files.Any(x => x.FullPath == f.FullPath))
                {
                    continue;
                }

                Files.Add(f);
            }
            Selected = Files.First();
        }

        private void Add()
        {
            var files = applicationManager.OpenFileDialog();
            if (files.Length != 0)
            {
                var parameter = new ShareFileViewModelParameter(warehouseService, blockchainService)
                {
                    FileModels = files.Select(f => new FileModel(f)).ToArray()
                };

                Prepare(parameter);
            }
        }

        private async Task Confirm()
        {
            await viewModelParameter.Confirm(Files.ToArray());
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
                    Selected = Files[filesCount - 1];
                }

                RaisePropertyChanged(nameof(FilesLength));
            }
        }
    }
}