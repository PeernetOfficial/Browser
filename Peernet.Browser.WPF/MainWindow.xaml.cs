using MvvmCross.Platforms.Wpf.Views;
using Peernet.Browser.Application;
using Peernet.Browser.Application.Contexts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Navigation;
using Peernet.Browser.Application.Services;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.ViewModels.Parameters;
using Peernet.Browser.Infrastructure.Services;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MvxWindow
    {
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

                var fileModels = (from path in paths where File.Exists(path) select new FileModel(path)).ToList();

                var parameter = new ShareFileViewModelParameter(Mvx.IoCProvider.Resolve<IWarehouseService>(), Mvx.IoCProvider.Resolve<IBlockchainService>())
                {
                    FileModels = fileModels
                };

                GlobalContext.IsMainWindowActive = false;
                Mvx.IoCProvider.Resolve<IMvxNavigationService>().Navigate<GenericFileViewModel, ShareFileViewModelParameter>(parameter);
            }
        }
    }
}