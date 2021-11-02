using MvvmCross.ViewModels;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class LoadingModel : MvxNotifyPropertyChanged
    {
        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set => SetProperty(ref isLoading, value);
        }

        private string text;

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public void Set(string text)
        {
            Text = text;
            IsLoading = true;
        }

        public void Reset()
        {
            Text = string.Empty;
            IsLoading = false;
        }
    }
}