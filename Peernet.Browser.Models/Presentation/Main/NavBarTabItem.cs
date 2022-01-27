namespace Peernet.Browser.Models.Presentation.Main
{
    public class NavBarTabItem
    {
        public NavBarTabItem(string title, object viewModel)
        {
            Title = title;
            ViewModel = viewModel;
        }

        public string Title { get; set; }

        public object ViewModel { get; set; }
    }
}
