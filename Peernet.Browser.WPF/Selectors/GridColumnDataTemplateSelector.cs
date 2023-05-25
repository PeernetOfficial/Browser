using System.Windows.Controls;
using System.Windows;
using Peernet.Browser.Application.ViewModels;

namespace Peernet.Browser.WPF.Selectors
{
    public class GridColumnDataTemplateSelector : DataTemplateSelector
    {
        //public static readonly DependencyProperty CurrentUserTabDataTemplateProperty =
        //    DependencyProperty.RegisterAttached("CurrentUserTabDataTemplate", typeof(DataTemplateSelector), typeof(GridColumnDataTemplateSelector),
        //        new FrameworkPropertyMetadata(new DataTemplate(), FrameworkPropertyMetadataOptions.Inherits));

        //public static readonly DependencyProperty UserTabDataTemplateProperty =
        //    DependencyProperty.RegisterAttached("UserTabDataTemplate", typeof(DataTemplateSelector), typeof(GridColumnDataTemplateSelector),
        //        new FrameworkPropertyMetadata(new DataTemplate(), FrameworkPropertyMetadataOptions.Inherits));

        //public DataTemplate CurrentUserTabDataTemplate
        //{
        //    get
        //    {
        //        return (DataTemplate)GetValue(CurrentUserTabDataTemplateProperty);
        //    }

        //    set
        //    {
        //        SetValue(CurrentUserTabDataTemplateProperty, value);
        //    }
        //}

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
