using Peernet.SDK.Models.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Peernet.SDK.Models.Extensions;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class FilesToCategoryBinder : IFilesToCategoryBinder
    {
        public List<VirtualFileSystemCoreCategory> Bind(IEnumerable<ApiFile> files)
        {
            var entities = files.Select(f => new VirtualFileSystemEntity(f));
            var remainingFiles = entities.ToList();
            List<VirtualFileSystemCoreCategory> categories = new();

            foreach (VirtualFileSystemEntityType type in Enum.GetValues(typeof(VirtualFileSystemEntityType)))
            {
                switch (type)
                {
                    case VirtualFileSystemEntityType.Picture:
                        {
                            AddCategory("Pictures", f => f.File.Type == LowLevelFileType.Picture);
                            break;
                        }
                    case VirtualFileSystemEntityType.Video:
                        {
                            AddCategory("Videos", f => f.File.Type == LowLevelFileType.Video);
                            break;
                        }
                    case VirtualFileSystemEntityType.Document:
                        {
                            AddCategory("Documents", f => f.File.Type == LowLevelFileType.Document);
                            break;
                        }
                    case VirtualFileSystemEntityType.Text:
                        {
                            AddCategory("Text", f => f.File.Type == LowLevelFileType.Text);
                            break;
                        }
                    case VirtualFileSystemEntityType.Audio:
                        {
                            AddCategory("Audio", f => f.File.Type == LowLevelFileType.Audio);
                            break;
                        }
                    case VirtualFileSystemEntityType.Ebook:
                        {
                            AddCategory("Ebook", f => f.File.Type == LowLevelFileType.Ebook);
                            break;
                        }
                }

                void AddCategory(
                    string categoryName,
                    Func<VirtualFileSystemEntity, bool> selector)
                {
                    var selectedFiles = entities.Where(selector).ToList();
                    categories.Add(new VirtualFileSystemCoreCategory(categoryName, type, selectedFiles));
                    remainingFiles.RemoveRange(selectedFiles);
                }
            }

            categories.Add(new VirtualFileSystemCoreCategory("Binary", VirtualFileSystemEntityType.Binary, remainingFiles));
            return categories;
        }
    }
}