﻿using System.ComponentModel;

namespace Peernet.Browser.Models.Presentation.Profile
{
    public class User : INotifyPropertyChanged
    {
        private byte[] image;
        private string name;

        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}