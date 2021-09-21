using System;
using System.Collections.Generic;
using System.Linq;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class FilesToCategoryBinder : IFilesToCategoryBinder
    {
        public List<VirtualFileSystemCategory> Bind(List<ApiBlockRecordFile> files)
        {
            var remainingFiles = files;
            List<VirtualFileSystemCategory> categories = new();

            foreach (VirtualFileSystemEntityType type in Enum.GetValues(typeof(VirtualFileSystemEntityType)))
            {
                switch (type)
                {
                    case VirtualFileSystemEntityType.Picture:
                    {
                        AddCategory("Pictures", f => f.Type == LowLevelFileType.Picture);
                        break;
                    }
                    case VirtualFileSystemEntityType.Video:
                    {
                        AddCategory("Videos", f => f.Type == LowLevelFileType.Video);
                        break;
                    }
                    case VirtualFileSystemEntityType.Document:
                    {
                        AddCategory("Documents", f => f.Type == LowLevelFileType.Document);
                        break;
                    }
                    case VirtualFileSystemEntityType.Text:
                    {
                        AddCategory("Text", f => f.Type == LowLevelFileType.Text);
                        break;
                    }
                    case VirtualFileSystemEntityType.Audio:
                    {
                        AddCategory("Audio", f => f.Type == LowLevelFileType.Audio);
                        break;
                    }
                    case VirtualFileSystemEntityType.Ebook:
                    {
                        AddCategory("Ebook", f => f.Format == HighLevelFileType.Ebook);
                        break;
                    }
                }

                void AddCategory(
                    string categoryName,
                    Func<ApiBlockRecordFile, bool> selector)
                {
                    var selectedFiles = files.Where(selector).ToList();
                    categories.Add(new VirtualFileSystemCategory(categoryName, type, selectedFiles));
                    remainingFiles.RemoveRange(selectedFiles);
                }

            }

            categories.Add(new VirtualFileSystemCategory("Binary", VirtualFileSystemEntityType.Binary, remainingFiles));
            return categories;
        }
    }
}
