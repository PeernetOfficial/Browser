namespace Peernet.Browser.Application.Managers
{
    public interface IApplicationManager
    {
        public void Shutdown();

        public void Maximize();

        public void Restore();

        public void Minimize();

        public bool IsMaximized { get; }

        string[] OpenFileDialog(bool multiselect = true, string filter = "");
    }
}