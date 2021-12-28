using MvvmCross;
using MvvmCross.Navigation;
using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Contexts;
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
    public partial class MainWindow : MvxWindow
    {
        private readonly object lockObject = new();
        
        public MainWindow()
        {
            InitializeComponent();
            MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            MouseDown += Window_MouseDown;

            //Hack for calendar
            CultureInfo ci = CultureInfo.CreateSpecificCulture("en-US");
            ci.DateTimeFormat.ShortestDayNames = new string[] { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
            ci.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Sunday;
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;

            // TODO: It should have better place. MainWindow's code behind is not the one. It should be called once UI(UI Thread Dispatcher) is created/initialized. Perhaps there is an even to handle.
            BindingOperations.EnableCollectionSynchronization(GlobalContext.Notifications, lockObject);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (newContent is IModal)
            {
                GlobalContext.Modal = newContent;
                Content = oldContent;
                return;
            }
            base.OnContentChanged(oldContent, newContent);
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (GlobalContext.CurrentViewModel != nameof(HomeViewModel))
            {
                Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<HomeViewModel>();
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

                var parameter = new ShareFileViewModelParameter(Mvx.IoCProvider.Resolve<IWarehouseService>(),
                    Mvx.IoCProvider.Resolve<IBlockchainService>())
                {
                    FileModels = fileModels
                };

                GlobalContext.IsMainWindowActive = false;
                Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<GenericFileViewModel, ShareFileViewModelParameter>(parameter);
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