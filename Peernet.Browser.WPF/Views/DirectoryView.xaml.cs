using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MvvmCross.Platforms.Wpf.Presenters.Attributes;
using MvvmCross.Platforms.Wpf.Views;
using MvvmCross.ViewModels;
using Peernet.Browser.Application.ViewModels;
using Peernet.Browser.Application.VirtualFileSystem;

namespace Peernet.Browser.WPF.Views
{
    /// <summary>
    /// Interaction logic for DictionaryView.xaml
    /// </summary>
    [MvxContentPresentation]
    [MvxViewFor(typeof(DirectoryViewModel))]
    public partial class DictionaryView : MvxWpfView
    {
        private Dictionary<VirtualFileSystemTier, TreeViewItem> pathElements;

        public DictionaryView()
        {
            InitializeComponent();
        }

        private void TreeViewItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var parentObject = GetParentObject((DependencyObject)e.OriginalSource);
            var tiers = GetAllTiersToTheTreeCore(parentObject);

            ((DirectoryViewModel)ViewModel).PathElements = new ObservableCollection<VirtualFileSystemTier>(tiers.Keys);

            pathElements = tiers;
        }

        private Dictionary<VirtualFileSystemTier, TreeViewItem> GetAllTiersToTheTreeCore(DependencyObject parentObject)
        {
            Dictionary<VirtualFileSystemTier, TreeViewItem> treeViewItems = new();
            string header = null;

            while (parentObject != null)
            {
                if (parentObject is TreeViewItem { DataContext: VirtualFileSystemTier tier } treeViewItem)
                {
                    treeViewItems.Add(tier, treeViewItem);
                    header ??= treeViewItem.Header.ToString();
                }
                
                parentObject = GetParentObject(parentObject);
            }

            treeViewItems = treeViewItems.Reverse().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return treeViewItems;
        }

        public static DependencyObject GetParentObject(DependencyObject child)
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

        private void SelectedTreeItem(object sender, RoutedEventArgs e)
        {
            var tier = (VirtualFileSystemTier)((FrameworkElement)e.OriginalSource).DataContext;
            var item = pathElements[tier];
            item.IsSelected = true;
            item.Focus();


            var elements = ((DirectoryViewModel)ViewModel).PathElements;
            var index = elements.IndexOf(tier);
            List<VirtualFileSystemTier> itemsToRemove = new();
            for (var i = index + 1; i < elements.Count; i++)
            {
                itemsToRemove.Add(elements[i]);
            }

            elements.RemoveRange(itemsToRemove);
        }
    }
}