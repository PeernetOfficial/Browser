using AsyncAwaitBestPractices.MVVM;
using Peernet.SDK.Models.Presentation;
using Peernet.SDK.Models.Presentation.Footer;
using System.Windows;
using System.Windows.Controls;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register(
                "File",
                typeof(DownloadModel),
                typeof(ChatControl),
                null);

        public static readonly DependencyProperty JoinChatCommandProperty =
            DependencyProperty.Register(
                "JoinChatCommand",
                typeof(IAsyncCommand<DownloadModel>),
                typeof(ChatControl),
                null);

        public ChatControl()
        {
            InitializeComponent();
        }

        public DownloadModel File
        {
            get => (DownloadModel)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        public IAsyncCommand<DownloadModel> JoinChatCommand
        {
            get => (IAsyncCommand<DownloadModel>)GetValue(JoinChatCommandProperty);
            set => SetValue(JoinChatCommandProperty, value);
        }
    }
}