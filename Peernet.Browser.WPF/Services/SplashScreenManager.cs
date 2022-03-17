namespace Peernet.Browser.WPF.Services
{
    public class SplashScreenManager
    {
        private PeernetSplashScreen splashScreen;

        public void Start()
        {
            splashScreen = new();
            splashScreen.Show();
        }

        public void SetState(string state)
        {
            splashScreen.CurrentState = state;
        }

        public void Exit()
        {
            splashScreen.Close();
        }
    }
}
