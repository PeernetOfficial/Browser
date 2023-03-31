using AsyncAwaitBestPractices.MVVM;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.SDK.Client.Clients;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Peernet.Browser.Application.ViewModels
{
    public class GenericFileViewModel<TParameter> : GenericViewModelBase<TParameter>, IModal
        where TParameter : FileParameterModel
    {
        private readonly IApplicationManager applicationManager;
        private readonly IModalNavigationService modalNavigationService;
        private readonly INavigationService navigationService;
        private readonly IBlockchainService blockchainService;
        private readonly IWarehouseClient warehouseClient;
        private readonly IFileService fileService;
        private readonly INotificationsManager notificationsManager;
        private readonly IDataTransferManager dataTransferManager;
        private readonly DirectoryViewModel directoryViewModel;
        private FileModel selected;
        private bool finishedProcessing = true;

        public GenericFileViewModel(
            IDataTransferManager dataTransferManager,
            INavigationService navigationService,
            IModalNavigationService modalNavigationService,
            IApplicationManager applicationManager,
            IBlockchainService blockchainClient,
            IWarehouseClient warehouseClient,
            IFileService fileService,
            INotificationsManager notificationsManager,
            DirectoryViewModel directoryViewModel)
        {
            this.dataTransferManager = dataTransferManager;
            this.modalNavigationService = modalNavigationService;
            this.navigationService = navigationService;
            this.applicationManager = applicationManager;
            this.blockchainService = blockchainClient;
            this.warehouseClient = warehouseClient;
            this.fileService = fileService;
            this.notificationsManager = notificationsManager;
            this.directoryViewModel = directoryViewModel;

            ConfirmCommand = new AsyncCommand(Confirm);
            CancelCommand = new AsyncCommand(() =>
            {
                Cancel();

                return Task.CompletedTask;
            });

            LeftCommand = new AsyncCommand(() =>
            {
                Manipulate(false);

                return Task.CompletedTask;
            });

            RightCommand = new AsyncCommand(() =>
            {
                Manipulate(true);

                return Task.CompletedTask;
            });

            AddCommand = new AsyncCommand(async () => await Add());

            DeleteFileCommand = new AsyncCommand(() =>
            {
                DeleteFile();

                return Task.CompletedTask;
            });

            Files.CollectionChanged += (s, o) =>
            {
                OnPropertyChanged(nameof(FilesLength));
                OnPropertyChanged(nameof(IsCountVisible));
            };
        }

        public IAsyncCommand AddCommand { get; }

        public IAsyncCommand ConfirmCommand { get; }

        public ObservableCollection<FileModel> Files { get; } = new ObservableCollection<FileModel>();

        public string FilesLength => $"{Files.IndexOf(Selected) + 1}/{Files.Count}";

        public IAsyncCommand CancelCommand { get; }

        public bool IsCountVisible => Files.Count > 1;

        public IAsyncCommand LeftCommand { get; }

        public IAsyncCommand RightCommand { get; }

        public IAsyncCommand DeleteFileCommand { get; }

        public FileModel Selected
        {
            get => selected ?? Files.FirstOrDefault();
            private set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }

        public bool FinishedProcessing
        {
            get => finishedProcessing;
            private set
            {
                finishedProcessing = value;
                OnPropertyChanged(nameof(FinishedProcessing));
            }
        }

        public string Title => Parameter.ModalTitle;

        public override async Task Prepare(TParameter parameter)
        {
            Parameter = parameter;
            foreach (var f in parameter.FileModels)
            {
                if (Files.Any(x => x.FullPath == f.FullPath))
                {
                    continue;
                }

                if (parameter.ShouldUpdateFormat)
                {
                    await UpdateFileFormat(f);
                }

                Files.Add(f);
            }
            Selected = Files.First();
        }

        private async Task Add()
        {
            var files = applicationManager.OpenFileDialog();
            if (files.Length != 0)
            {
                var parameter = new ShareFileViewModelParameter(dataTransferManager, warehouseClient, blockchainService, navigationService, notificationsManager, directoryViewModel)
                {
                    FileModels = files.Select(f => new FileModel(f)).ToList()
                };

                await Prepare(parameter as TParameter);
            }
        }

        private Task Confirm()
        {
            FinishedProcessing = false;
            Task.Run(() => Parameter.Confirm(Files.ToArray()));
            
            FinishedProcessing = true;
            Cancel();

            return Task.CompletedTask;
        }

        private void Cancel()
        {
            modalNavigationService.Close();
        }

        private void Manipulate(bool isPlus)
        {
            var i = isPlus ? 1 : -1;
            var index = Files.IndexOf(Selected) + i;
            if (index >= 0 && index <= Files.Count - 1)
            {
                Selected = Files[index];
                OnPropertyChanged(nameof(FilesLength));
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

                OnPropertyChanged(nameof(FilesLength));
            }
        }

        private async Task UpdateFileFormat(FileModel fileModel)
        {
            var format = await fileService.GetFormat(fileModel.FullPath);
            if (format != null)
            {
                fileModel.Format = format.FileFormat;
                fileModel.Type = format.FileType;
            }
        }
    }
}