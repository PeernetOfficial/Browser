using Peernet.SDK.Models.Domain.Common;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for GenericEmbededPluginControl.xaml
    /// </summary>
    public partial class GenericEmbededPluginControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register(
                "File",
                typeof(ApiFile),
                typeof(GenericEmbededPluginControl),
                null);

        public GenericEmbededPluginControl()
        {
            InitializeComponent();
        }

        public ApiFile File
        {
            get => (ApiFile)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }
    }
}