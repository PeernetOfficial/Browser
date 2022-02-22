using System;
using System.ComponentModel;

namespace Peernet.Browser.Application.Contexts
{
    public class PluginsContext : INotifyPropertyChanged
    {
        private static bool playButtonPlugEnabled;

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };

        public event PropertyChangedEventHandler PropertyChanged;

        public static bool PlayButtonPlugEnabled
        {
            get => playButtonPlugEnabled;
            set
            {
                playButtonPlugEnabled = value;
                NotifyStaticPropertyChanged(nameof(PlayButtonPlugEnabled));
            }
        }

        private static void NotifyStaticPropertyChanged(string propertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(propertyName));
        }
    }
}