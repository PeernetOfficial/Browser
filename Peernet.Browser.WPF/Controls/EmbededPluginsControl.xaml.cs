using Peernet.SDK.Models.Domain.Common;
using System.Windows;
using System.Windows.Controls;
using Peernet.SDK.Models.Plugins;
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
    public partial class EmbededPluginsControl : UserControl
    {
        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register("File", typeof(ApiFile),
        typeof(EmbededPluginsControl), null);

        public EmbededPluginsControl()
        {
            InitializeComponent();
            Loaded += EmbededPluginsControl_Loaded;
        }

        private void EmbededPluginsControl_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = App.ServiceProvider.GetService(typeof(ISettingsManager)) as ISettingsManager;
            Player.Source = GetFileSource(settings, File);
        }

        public ApiFile File
        {
            get => (ApiFile)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        void OnMouseDownPlayMedia(object sender, MouseButtonEventArgs args)
        {

            // The Play method will begin the media if it is not currently active or
            // resume media if it is paused. This has no effect if the media is
            // already running.
            Player.Play();
            PlayButton.Visibility = Visibility.Collapsed;
            PauseButton.Visibility = Visibility.Visible;

            // Initialize the MediaElement property values.
            InitializePropertyValues();
        }

        // Pause the media.
        void OnMouseDownPauseMedia(object sender, MouseButtonEventArgs args)
        {

            // The Pause method pauses the media if it is currently running.
            // The Play method can be used to resume.
            Player.Pause();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        // Stop the media.
        void OnMouseDownStopMedia(object sender, MouseButtonEventArgs args)
        {

            // The Stop method stops and resets the media to be played from
            // the beginning.
            Player.Stop();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        // Change the volume of the media.
        private void ChangeMediaVolume(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            Player.Volume = (double)volumeSlider.Value;
        }

        // Change the speed of the media.
        private void ChangeMediaSpeedRatio(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            Player.SpeedRatio = (double)speedRatioSlider.Value;
        }

        // When the media opens, initialize the "Seek To" slider maximum value
        // to the total number of miliseconds in the length of the media clip.
        private void Element_MediaOpened(object sender, EventArgs e)
        {
            timelineSlider.Maximum = Player.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        // When the media playback is finished. Stop() the media to seek to media start.
        private void Element_MediaEnded(object sender, EventArgs e)
        {
            Player.Stop();
            PauseButton.Visibility = Visibility.Collapsed;
            PlayButton.Visibility = Visibility.Visible;
        }

        // Jump to different parts of the media (seek to).
        private void SeekToMediaPosition(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            Player.Position = ts;
        }

        void InitializePropertyValues()
        {
            // Set the media's starting Volume and SpeedRatio to the current value of the
            // their respective slider controls.
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
    }
}