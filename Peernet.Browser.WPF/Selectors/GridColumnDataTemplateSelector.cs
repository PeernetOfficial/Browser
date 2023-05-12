using System.Windows.Controls;
using System.Windows;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Selectors
{
    public class GridColumnDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;
            if (item is UserDirectoryViewModel)
            {
                return element.FindResource("userDiectoryDataGridControl") as DataTemplate;
            }
            else if (item is CurrentUserDirectoryViewModel)
            {
                return element.FindResource("currentUserDiectoryDataGridControl") as DataTemplate;
            }
            else
            {
                return null;
            }
        }
    }
}
