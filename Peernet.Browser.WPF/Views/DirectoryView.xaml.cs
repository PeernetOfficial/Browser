using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.VirtualFileSystem;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for DictionaryView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(DirectoryViewModel))]
    public partial class DirectoryView : MvxWpfView
    {
        private Dictionary<VirtualFileSystemEntity, TreeViewItem> pathElements;

        public DirectoryView()
        {
            InitializeComponent();
        }

        private static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null)
            {
                return null;
            }

            //handle content elements separately
            if (child is ContentElement contentElement)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null)
                {
                    return parent;
                }

                return contentElement is FrameworkContentElement fce ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            if (child is FrameworkElement frameworkElement)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null)
                {
                    return parent;
                }
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        private Dictionary<VirtualFileSystemEntity, TreeViewItem> GetAllEntitiesToTheTreeCore(DependencyObject parentObject)
        {
            Dictionary<VirtualFileSystemEntity, TreeViewItem> treeViewItems = new();

            while (parentObject != null)
            {
                if (parentObject is TreeViewItem { DataContext: VirtualFileSystemEntity entity } treeViewItem)
                {
                    entity.IsVisualTreeVertex = false;
                    treeViewItems.Add(entity, treeViewItem);
                }

                if (parentObject is TreeViewItem { DataContext: DirectoryViewModel } treeView)
                {
                    treeViewItems.TryAdd(
                        new VirtualFileSystemTier(treeView.Header.ToString(), VirtualFileSystemEntityType.All, -1), treeView);
                }

                parentObject = GetParentObject(parentObject);
            }

            treeViewItems.First().Key.IsVisualTreeVertex = true;
            treeViewItems = treeViewItems.Reverse().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return treeViewItems;
        }

        private void PathSegment_OnSelected(object sender, RoutedEventArgs e)
        {
            var entity = (VirtualFileSystemEntity)((FrameworkElement)e.OriginalSource).DataContext;
            var item = pathElements[entity];
            item.IsSelected = true;

            var viewModel = (DirectoryViewModel)ViewModel;
            viewModel.ChangeSelectedEntity(entity);
            item.Focus();

            var elements = viewModel.PathElements;
            var index = elements.IndexOf(entity);
            List<VirtualFileSystemEntity> itemsToRemove = new();
            for (var i = index + 1; i < elements.Count; i++)
            {
                itemsToRemove.Add(elements[i]);
            }

            elements.RemoveRange(itemsToRemove);
            elements.Last().IsVisualTreeVertex = true;
        }

        private void TreeViewItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var parentObject = GetParentObject((DependencyObject)e.OriginalSource);
            var entities = GetAllEntitiesToTheTreeCore(parentObject);

            var viewModel = (DirectoryViewModel)ViewModel;
            viewModel.PathElements = new ObservableCollection<VirtualFileSystemEntity>(entities.Keys);

            viewModel.ChangeSelectedEntity(entities.Last().Key);
            pathElements = entities;
        }
    }
}