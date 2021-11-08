using Microsoft.Win32;
using MvvmCross;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.WPF.Views;
using System;
using System.Collections.Generic;

namespace Peernet.Browser.WPF.Services
{
    public class ApplicationManager : IApplicationManager
    {
        private readonly IDictionary<ViewType, MvxWpfView> viewContainer = new Dictionary<ViewType, MvxWpfView>();

        public ApplicationManager()
        {
            SetWindow();
        }

        private MainWindow window;

        private void SetWindow()
        {
            if (window == null) window = (MainWindow)System.Windows.Application.Current.MainWindow;
        }

        public bool IsMaximized
        {
            get => window.WindowState == System.Windows.WindowState.Maximized;
        }

        public void Init()
        {
            NavigateToMain(ViewType.Home);
        }

        public void Maximize() => window.WindowState = System.Windows.WindowState.Maximized;

        public void Minimize() => window.WindowState = System.Windows.WindowState.Minimized;

        public void Shutdown() => window.Close();

        public void Restore() => window.WindowState = System.Windows.WindowState.Normal;

        public string[] OpenFileDialog(bool multiselect = true, string filter = "")
        {
            var dialog = new OpenFileDialog { Multiselect = multiselect };
            dialog.Filter = filter;
            if (dialog.ShowDialog().GetValueOrDefault()) return dialog.FileNames;
            else return new string[0];
        }

        private MvxWpfView GetView(ViewType v)
        {
            if (!viewContainer.ContainsKey(v))
            {
                MvxWpfView toAdd;
                switch (v)
                {
                    case ViewType.Home:
                        toAdd = GetView<HomeView>();
                        break;

                    case ViewType.Directory:
                        toAdd = GetView<DirectoryView>();
                        break;

                    case ViewType.Explorer:
                        toAdd = GetView<ExploreView>();
                        break;

                    case ViewType.About:
                        toAdd = GetView<AboutView>();
                        break;

                    case ViewType.Filter:
                        toAdd = GetView<FiltersView>();
                        break;

                    case ViewType.EditProfile:
                        toAdd = GetView<EditProfileView>();
                        break;

                    case ViewType.GenericFile:
                        toAdd = GetView<GenericFileView>();
                        break;

                    case ViewType.DeleteAccount:
                        toAdd = GetView<DeleteAccountView>();
                        break;

                    case ViewType.None:
                        toAdd = null;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                viewContainer.Add(v, toAdd);
            }
            return viewContainer[v];
        }

        private T GetView<T>() where T : MvxWpfView, new()
        {
            var view = new T();
            if (view == null) throw new ArgumentException();
            var attr = (MvxViewForAttribute)Attribute.GetCustomAttribute(view.GetType(), typeof(MvxViewForAttribute));
            var viewModel = Mvx.IoCProvider.Resolve(attr.ViewModel) as IMvxViewModel;
            view.ViewModel = viewModel;
            return view;
        }

        public void NavigateToMain(ViewType v, bool showLogo = true)
        {
            var view = GetView(v);
            var newView = view.ViewModel.GetType().Name;
            if (GlobalContext.CurrentViewModel == newView)
            {
                return;
            }
            GlobalContext.Main = view;
            GlobalContext.IsLogoVisible = showLogo;
            GlobalContext.CurrentViewModel = newView;
        }

        public void NavigateToModal(ViewType v, object model = null)
        {
            GlobalContext.IsMainWindowActive = false;
            GlobalContext.IsProfileMenuVisible = false;
            var view = GetView(v);
            GlobalContext.Modal = view;
            if (model == null) return;
            var methodInfo = view.ViewModel.
                GetType().
                GetMethod("Prepare", new[] { model.GetType() });
            methodInfo.Invoke(view.ViewModel, new[] { model });
        }

        public void CloseModal()
        {
            GlobalContext.IsMainWindowActive = true;
            GlobalContext.IsProfileMenuVisible = false;
            GlobalContext.Modal = GetView(ViewType.None);
        }
    }
}