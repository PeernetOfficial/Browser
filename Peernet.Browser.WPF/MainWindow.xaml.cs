using Microsoft.Extensions.DependencyInjection;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Dispatchers;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Application.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Models.Presentation.Footer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly object lockObject = new();
        
        public MainWindow(object dataContext)
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            DataContext = dataContext;
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MouseDown += Window_MouseDown;

            //Hack for calendar
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            ci.DateTimeFormat.ShortestDayNames = new string[] { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
            ci.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            // TODO: It should have better place. MainWindow's code behind is not the one. It should be called once UI(UI Thread Dispatcher) is created/initialized. Perhaps there is an even to handle.
            BindingOperations.EnableCollectionSynchronization(App.ServiceProvider.GetRequiredService<INotificationsManager>().Notifications, lockObject);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UIThreadDispatcher.SetUIContext(SynchronizationContext.Current);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalContext.CurrentViewModel != nameof(HomeViewModel))
            {
                App.ServiceProvider.GetRequiredService<INavigationService>().Navigate<HomeViewModel>();
                GlobalContext.CurrentViewModel = nameof(HomeViewModel);
            }
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

                var modalNavigationService = App.ServiceProvider.GetRequiredService<IModalNavigationService>();
                var parameter = new ShareFileViewModelParameter(
                    App.ServiceProvider.GetRequiredService<IWarehouseService>(),
                    App.ServiceProvider.GetRequiredService<IBlockchainService>(),
                    modalNavigationService,
                    App.ServiceProvider.GetRequiredService<INotificationsManager>(),
                    App.ServiceProvider.GetRequiredService<DirectoryViewModel>())
                {
                    FileModels = fileModels
                };

                modalNavigationService.Navigate<ShareFileViewModel, ShareFileViewModelParameter>(parameter);
            }
        }

        private void MainWindow_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            App.RaiseMainWindowClick(sender, e);
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
    }
}