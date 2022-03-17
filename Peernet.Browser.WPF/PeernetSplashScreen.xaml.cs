using System.Windows;

namespace Peernet.Browser.WPF
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class PeernetSplashScreen : Window
    {
        public static readonly DependencyProperty CurrentStateProperty = DependencyProperty.Register("CurrentState", typeof(string), typeof(PeernetSplashScreen));

        public string CurrentState 
        {
            get => (string)GetValue(CurrentStateProperty);
            set
            {
                SetValue(CurrentStateProperty, value);
            }
        }

        public PeernetSplashScreen()
        {
            Loaded += PeernetSplashScreen_Loaded;
            InitializeComponent();
        }

        private void PeernetSplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateLayout();
        }
    }
}
