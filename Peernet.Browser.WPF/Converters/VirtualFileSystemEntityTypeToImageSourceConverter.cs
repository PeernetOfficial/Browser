﻿using System;
using System.Globalization;
using MvvmCross.Converters;
using MvvmCross.Platforms.Wpf.Converters;
using Peernet.Browser.Application.VirtualFileSystem;

namespace Peernet.Browser.WPF.Converters
{
    internal class
        VirtualFileSystemEntityTypeToImageSourceConverter : MvxNativeValueConverter<
            VirtualFileSystemEntityTypeToImageSourceValueConverter>
    {
    }

    internal class VirtualFileSystemEntityTypeToImageSourceValueConverter : MvxValueConverter<VirtualFileSystemEntityType, string>
    {
        protected override string Convert(
            VirtualFileSystemEntityType value,
            Type targetType,
            object parameter,
            CultureInfo? culture)
        {
            string asset = null;
            switch (value)
            {
                case VirtualFileSystemEntityType.All:
                    asset = GetAsset("AllFiles.png");
                    break;
                case VirtualFileSystemEntityType.Recent:
                    asset = GetAsset("Recent.png");
                    break;
                case VirtualFileSystemEntityType.Audio:
                    asset = GetAsset("Audio.png");
                    break;
                case VirtualFileSystemEntityType.Video:
                    asset = GetAsset("Video.png");
                    break;
                case VirtualFileSystemEntityType.Binary:
                    asset = GetAsset("Binary.png");
                    break;
                case VirtualFileSystemEntityType.Picture:
                    asset = GetAsset("Picture.png");
                    break;
                case VirtualFileSystemEntityType.Document:
                    asset = GetAsset("Document.png");
                    break;
                case VirtualFileSystemEntityType.Directory:
                    asset = GetAsset("File.png");
                    break;
                case VirtualFileSystemEntityType.Text:
                    asset = GetAsset("Text.png");
                    break;
                case VirtualFileSystemEntityType.Ebook:
                    asset = GetAsset("Ebook.png");
                    break;
            }

            return asset;
        }

        private string GetAsset(string assetName)
        {
            return $"/Assets/{assetName}";
        }
    }
}