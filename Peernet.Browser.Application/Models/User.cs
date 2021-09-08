using System.ComponentModel;

namespace Peernet.Browser.Application.Models
{
    public class User : INotifyPropertyChanged
    {
        private byte[] image;
        private string name;

        public event PropertyChangedEventHandler PropertyChanged;

        public User()
        {
        }

        public byte[] Image
        {
            get => image;
            set
            {
                image = value;
                NotifyPropertyChanged(nameof(Image));
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged(nameof(Name));
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public User GetClone()
        {
            return (User)MemberwiseClone();
        }
    }
}