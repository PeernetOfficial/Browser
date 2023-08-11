using System.Windows.Media;
using System.Windows;

namespace Peernet.Browser.WPF.Extensions
{
    public static class ObjectExtensions
    {
        public static T GetVisualTreeParent<T>(this object originalSource) where T : class
        {
            var dependencyObject = originalSource as DependencyObject;

            while (dependencyObject != null)
            {
                var control = dependencyObject as T;
                if (control != null)
                {
                    return control;
                }

                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            return null;
        }
    }
}
