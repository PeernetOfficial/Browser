using Peernet.Browser.Application.ViewModels;
using Peernet.SDK.Models.Presentation.Profile;
using Peernet.SDK.WPF;
using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for UserProfileWindow.xaml
    /// </summary>
    public partial class UserProfileWindow : PeernetWindow
    {
        public UserProfileWindow(UserProfileViewModel userProfileViewModel)
        {
            InitializeComponent();
            DataContext = userProfileViewModel;
        }
    }
}