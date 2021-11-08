namespace Peernet.Browser.Application.Managers
{
    public enum ViewType
    {
        None,
        Home,
        Directory,
        Explorer,
        About,
        Filter,
        EditProfile,
        GenericFile
    }

    public interface IApplicationManager
    {
        public void Shutdown();

        public void Maximize();

        public void Restore();

        public void Minimize();

        public bool IsMaximized { get; }

        string[] OpenFileDialog(bool multiselect = true, string filter = "");

        void Init();

        void NavigateToModal(ViewType v, object model = null);

        void NavigateToMain(ViewType v, bool showLogo = true);

        void CloseModal();
    }
}