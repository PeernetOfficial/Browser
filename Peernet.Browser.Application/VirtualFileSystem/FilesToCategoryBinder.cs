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
                        var selectedFiles = files.Where(f => f.Type == LowLevelFileType.Picture).ToList();
                        categories.Add(new VirtualFileSystemCategory("Pictures", type, selectedFiles));
                        RemoveFilesFromCollection(remainingFiles, selectedFiles);
                        break;
                    }
                    case VirtualFileSystemEntityType.Video:
                    {
                        var selectedFiles = files.Where(f => f.Type == LowLevelFileType.Video).ToList();
                        categories.Add(new VirtualFileSystemCategory("Videos", type, selectedFiles));
                        RemoveFilesFromCollection(remainingFiles, selectedFiles);
                        break;
                    }
                    case VirtualFileSystemEntityType.Document:
                    {
                        var selectedFiles = files.Where(f => f.Type == LowLevelFileType.Document).ToList();
                        categories.Add(new VirtualFileSystemCategory("Documents", type, selectedFiles));
                        RemoveFilesFromCollection(remainingFiles, selectedFiles);
                        break;
                    }
                    case VirtualFileSystemEntityType.Text:
                    {
                        var selectedFiles = files.Where(f => f.Type == LowLevelFileType.Text).ToList();
                        categories.Add(new VirtualFileSystemCategory("Text", type, selectedFiles));
                        RemoveFilesFromCollection(remainingFiles, selectedFiles);
                        break;
                    }
                    case VirtualFileSystemEntityType.Audio:
                    {
                        var selectedFiles = files.Where(f => f.Type == LowLevelFileType.Audio).ToList();
                        categories.Add(new VirtualFileSystemCategory("Audio", type, selectedFiles));
                        RemoveFilesFromCollection(remainingFiles, selectedFiles);
                        break;
                    }
                }

            }

            categories.Add(new VirtualFileSystemCategory("Binary", VirtualFileSystemEntityType.Binary, remainingFiles));
            return categories;
        }

        private void RemoveFilesFromCollection(List<ApiBlockRecordFile> collection,
            List<ApiBlockRecordFile> filesToRemove)
        {
            foreach (var selectedFile in filesToRemove)
            {
                collection.Remove(selectedFile);
            }
        }
    }
}
