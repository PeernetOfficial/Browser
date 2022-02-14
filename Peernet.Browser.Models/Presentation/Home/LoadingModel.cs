using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Home
{
    public class LoadingModel : INotifyPropertyChanged
    {
        private bool isLoading;

        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                PropertyChanged?.Invoke(this, new(nameof(IsLoading)));
                PropertyChanged?.Invoke(this, new(nameof(IsNotLoading)));
            }
        }

        public bool IsNotLoading => !IsLoading;

        private string text;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new(nameof(Text)));
            }
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