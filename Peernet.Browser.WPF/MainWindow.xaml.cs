using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.WPF.Views;
using Peernet.SDK.Models.Extensions;
using Peernet.SDK.Models.Presentation.Footer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel dataContext)
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            DataContext = dataContext;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MouseDown += Window_MouseDown;
            if (DataContext is MainViewModel main)
            {
                main.OpenAboutTab = () => AboutTab.IsSelected = true;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UIThreadDispatcher.SetUIContext(SynchronizationContext.Current);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            HomeTab.IsSelected = true;
        }

        private void FileUpload_OnDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var paths = (string[])e.Data.GetData(DataFormats.FileDrop);

                var fileModels = new List<FileModel>();
                foreach (var path in paths)
                {
                    if (File.Exists(path))
                    {
                        fileModels.Add(new FileModel(path));
                    }
                    else if (Directory.Exists(path))
                    {
                        fileModels.AddRange(GetAllDescendantFiles(path));
                    }
                }

                var directoryViewModel = App.ServiceProvider.GetRequiredService<DirectoryViewModel>();
                if (DirectoryTab.IsSelected)
                {
                    var selected = directoryViewModel.VirtualFileSystem.GetCurrentlySelected();
                    if (selected is not VirtualFileSystemCoreCategory && selected is not VirtualFileSystemCoreTier { Name: "Recent" } && selected is not VirtualFileSystemCoreTier { Name: "All files" })
                    {
                        fileModels.ForEach(fm => fm.Directory = ChangeFileLocationViewModel.TrimUnsopportedSegments(selected.AbsolutePath));
                    }
                }

                var modalNavigationService = App.ServiceProvider.GetRequiredService<IModalNavigationService>();
                var parameter = new ShareFileViewModelParameter(
                    App.ServiceProvider.GetRequiredService<IWarehouseService>(),
                    App.ServiceProvider.GetRequiredService<IBlockchainService>(),
                    modalNavigationService,
                    App.ServiceProvider.GetRequiredService<INotificationsManager>(),
                    directoryViewModel)
                {
                    FileModels = fileModels
                };

                modalNavigationService.Navigate<ShareFileViewModel, ShareFileViewModelParameter>(parameter);
            }
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            App.RaiseMainWindowClick(sender, e);
            Keyboard.ClearFocus();
            Main.Focus();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private List<FileModel> GetAllDescendantFiles(string path, string rootFolder = null)
        {
            var fileModels = new List<FileModel>();
            var files = Directory.GetFiles(path);
            var currentFolder = path.Split("\\").Last();
            var folderTree = rootFolder == null ? currentFolder : Path.Combine(rootFolder, currentFolder);
            files.Foreach(f => fileModels.Add(new FileModel(f, folderTree)));
            var directories = Directory.GetDirectories(path);
            foreach (var directoryPath in directories)
            {
                fileModels.AddRange(GetAllDescendantFiles(directoryPath, folderTree));
            }

            return fileModels;
        }

        private void TabControlEx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count != 0 && e.AddedItems[0] is TabItem tab)
            {
                if (tab.Content is DirectoryView or ExploreView or AboutView)
                {
                    GlobalContext.IsLogoVisible = true;
                }
                else if (tab.Content is HomeView)
                {
                    GlobalContext.IsLogoVisible = App.ServiceProvider.GetService<HomeViewModel>().IsVisible;
                }
                else
                {
                    GlobalContext.IsLogoVisible = false;
                }
            }
        }
    }
}