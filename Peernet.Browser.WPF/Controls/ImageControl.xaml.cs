using Peernet.SDK.Client.Http;
using Peernet.SDK.Common;
using Peernet.SDK.Models.Domain.Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ImageControl.xaml
    /// </summary>
    public partial class ImageControl : UserControl
    {

        public static readonly DependencyProperty FileProperty =
            DependencyProperty.Register(
                "File",
                typeof(ApiFile),
                typeof(ImageControl),
                null);


        private void ImageControl_Loaded(object sender, RoutedEventArgs e)
        {
            var settings = App.ServiceProvider.GetService(typeof(ISettingsManager)) as ISettingsManager;
            ImageContent.Source = new BitmapImage(GetFileSource(settings, File));
        }

        public ApiFile File
        {
            get => (ApiFile)GetValue(FileProperty);
            set => SetValue(FileProperty, value);
        }

        public ImageControl()
        {
            InitializeComponent();
            Loaded += ImageControl_Loaded;
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
