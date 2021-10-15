using Peernet.Browser.Application.Contexts;
using Peernet.Browser.Models.Presentation;
using System;
using System.Windows;

namespace Peernet.Browser.WPF.Styles
{
    public class ModeResourceDictionary : ResourceDictionary
    {
        private Uri lightModeSource;
        private Uri darkModeSource;

        public Uri LightModeSource
        {
            get { return lightModeSource; }
            set
            {
                lightModeSource = value;
                UpdateSource();
            }
        }
        public Uri DarkModeSource
        {
            get { return darkModeSource; }
            set
            {
                darkModeSource = value;
                UpdateSource();
            }
        }

        public void UpdateSource()
        {
            var mode = GlobalContext.VisualMode == VisualMode.LightMode ? LightModeSource : DarkModeSource;
            if (mode != null && Source != mode)
            {
                Source = mode;
            }
        }
    }
}