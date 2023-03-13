using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Media.Animation;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class PeernetSplashScreen : Window
    {
        public PeernetSplashScreen()
        {
            InitializeComponent();
        }

        public RepeatBehavior RepeatBehavior => new RepeatBehavior(1);

        private void Intro_AnimationCompleted(object sender, RoutedEventArgs e)
        {
            var mainWindow = App.ServiceProvider.GetRequiredService<MainWindow>();
            App.Current.MainWindow = mainWindow;
            mainWindow.Show();
            this.Close();
        }
    }
}