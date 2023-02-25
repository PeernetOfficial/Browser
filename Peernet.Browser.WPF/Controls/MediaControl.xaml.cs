using Peernet.SDK.Models.Domain.Common;
using System.Windows;
using System.Windows.Controls;
using Peernet.SDK.Client.Http;
using Peernet.SDK.Common;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Windows.Input;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for EmbededPluginsControl.xaml
    /// </summary>
    public partial class MediaControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(ApiFile),
        typeof(MediaControl), null);

        public MediaControl()
        {
            InitializeComponent();
            Loaded += EmbededPluginsControl_Loaded;
        }

        private void EmbededPluginsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = App.ServiceProvider.GetService(typeof(ISettingsManager)) as ISettingsManager;
            Player.Source = GetFileSource(settings, File);
            LoadPreview(Player);
        }

        public ApiFile File
        {
            get => (ApiFile)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {
            Player.Play();
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;

            InitializePropertyValues();
        }

        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {
            Player.Pause();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        // Stop the media.
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {
            Player.Stop();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            Player.Volume = (double)volumeSlider.Value;
        }

        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            Player.SpeedRatio = (double)speedRatioSlider.Value;
        }

        private void Element_MediaOpened(object sender, EventArgs e)
        {
            //timelineSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void Element_MediaEnded(object sender, EventArgs e)
        {
            Player.Stop();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            //int SliderValue = (int)timelineSlider.Value;

            //TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            //Player.Position = ts;
        }

        void InitializePropertyValues()
        {
            Player.Volume = (double)volumeSlider.Value;
            Player.SpeedRatio = (double)speedRatioSlider.Value;
        }

        public static Uri GetFileSource(ISettingsManager settingsManager, ApiFile file)
        {
            var parameters = new Dictionary<string, string>
            {
                ["hash"] = Convert.ToHexString(file.Hash),
                ["node"] = Convert.ToHexString(file.NodeId),
                ["format"] = "14",
                ["k"] = settingsManager.ApiKey
            };

            var uriBase = $"{settingsManager.ApiUrl}/file/view";

            var requestMessage = HttpHelper.PrepareMessage(uriBase, HttpMethod.Get, parameters, null);
            return requestMessage.RequestUri;
        }

        private static void LoadPreview(MediaElement Player)
        {
            Player.ScrubbingEnabled = true;
            Player.Position = TimeSpan.FromMilliseconds(1);
            Player.Volume = 0;
            Player.IsMuted = true;
            Player.Play();
            Player.Stop();
            Player.IsMuted = false;
            Player.Volume = 0.5;
        }
    }
}