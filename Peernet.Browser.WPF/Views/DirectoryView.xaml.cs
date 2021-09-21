using System.Collections.Generic;
using System.Diagnostics;
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
        public DictionaryView()
        {
            InitializeComponent();
        }

        private void TreeViewItem_OnSelected(object sender, RoutedEventArgs e)
        {
            var parentObject = GetParentObject((DependencyObject)e.OriginalSource);
            ((DirectoryViewModel)ViewModel).PathElements = GetAllTiersToTheTreeCore(parentObject).Reverse().ToList();
        }

        private HashSet<VirtualFileSystemTier> GetAllTiersToTheTreeCore(DependencyObject parentObject)
        {
            HashSet<VirtualFileSystemTier> treeViewItems = new();

            while (parentObject != null)
            {
                if ((parentObject as FrameworkElement).DataContext is VirtualFileSystemTier vfst)
                {
                    treeViewItems.Add(vfst);
                }
                
                parentObject = GetParentObject(parentObject);
            }

            return treeViewItems;
        }

        public static DependencyObject GetParentObject(DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            if (child is ContentElement contentElement)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                return contentElement is FrameworkContentElement fce ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            if (child is FrameworkElement frameworkElement)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }
    }
}