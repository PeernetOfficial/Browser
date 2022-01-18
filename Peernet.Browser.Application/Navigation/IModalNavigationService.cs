namespace Peernet.Browser.Application.Navigation
{
    public interface IModalNavigationService : INavigationService
    {
        bool IsOpen { get; }

        void Close();
    }
}