using Peernet.Browser.Models.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Peernet.Browser.Models.Presentation.Footer
{
    public class FileModel : INotifyPropertyChanged
    {
        private DateTime createDate;
        private string description;
        private string directory;
        private string fileNameWithoutExtension;
        private string fullPath;
        private long size;
        private byte[] hash;
        private string extension;

        public event PropertyChangedEventHandler PropertyChanged;

        public FileModel(string path, string directory = null)
        {
            var f = new FileInfo(path);
            FullPath = f.FullName;
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(f.Name);
            Extension = Path.GetExtension(f.Name);
            Size = f.Length;
            CreateDate = DateTime.Now;
            Directory = directory;
        }

        public FileModel(ApiFile apiFile)
        {
            Id = apiFile.Id;
            NodeId = apiFile.NodeId;
            Size = apiFile.Size;
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(apiFile.Name);
            Extension = Path.GetExtension(apiFile.Name);
            Hash = apiFile.Hash;
            Metadata = apiFile.MetaData;
            Directory = apiFile.Folder;
            Format = apiFile.Format;
            Type = apiFile.Type;
            Description = apiFile.Description;
            CreateDate = DateTime.Now;
        }

        public DateTime CreateDate
        {
            get => createDate;
            set
            {
                createDate = value;
                PropertyChanged?.Invoke(this, new(nameof(CreateDate)));
            }
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                PropertyChanged?.Invoke(this, new(nameof(Description)));
            }
        }

        public string Directory
        {
            get => directory;
            set
            {
                directory = value;
                PropertyChanged?.Invoke(this, new(nameof(Directory)));
            }
        }

        public string FileNameWithoutExtension
        {
            get => fileNameWithoutExtension;
            set
            {
                fileNameWithoutExtension = value;
                PropertyChanged?.Invoke(this, new(nameof(FileNameWithoutExtension)));
            }
        }
        
        public string Extension
        {
            get => extension;
            set
            {
                extension = value;
                PropertyChanged?.Invoke(this, new(nameof(Extension)));
            }
        }

        public string FullPath
        {
            get => fullPath;
            set
            {
                fullPath = value;
                PropertyChanged?.Invoke(this, new(nameof(FullPath)));
            }
        }

        public string FormattedSize => GetSizeString(Size);

        public string Id { get; set; }

        public byte[] Hash
        {
            get => hash;
            set
            {
                hash = value;
                PropertyChanged?.Invoke(this, new(nameof(Hash)));
            }
        }

        public List<ApiFileMetadata> Metadata { get; set; }

        public byte[] NodeId { get; set; }

        public long Size
        {
            get => size;
            set
            {
                size = value;
                PropertyChanged?.Invoke(this, new(nameof(Size)));
            }
        }

        public LowLevelFileType Type { get; set; }

        public HighLevelFileType Format { get; set; }

        private string GetSizeString(long o)
        {
            var len = o;
            var sizes = new[] { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{o} bytes ({len:0.##} {sizes[order]})";
        }
    }
}