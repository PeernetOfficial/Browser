using MvvmCross.Plugin.Control.Platforms.Wpf;
using Peernet.Browser.Application.Download;
using Peernet.Browser.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Peernet.Browser.Models.Domain.Download;
using Peernet.Browser.Models.Presentation.Footer;

namespace Peernet.Browser.WPF.Controls
{
    /// <summary>
    /// Interaction logic for Footer.xaml
    /// </summary>
    public partial class FooterControl : MvxWpfControl
    {
        private readonly IDownloadManager downloadManager;

        public FooterControl()
        {
            InitializeComponent();
        }
    }
}