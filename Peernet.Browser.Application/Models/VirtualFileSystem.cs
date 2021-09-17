using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class VirtualFileSystem
    {
        public VirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles)
        {
            CreateFileSystemStructure(sharedFiles);
        }

        public List<VirtualFileSystemTier> VirtualFileSystemTiers { get; set; } = new();

        public List<VirtualFileSystemCategory> VirtualFileSystemCategories { get; set; } = new();

        // Organize Files into the structured system
        private void CreateFileSystemStructure(IEnumerable<ApiBlockRecordFile> sharedFiles)
        {
            // materialize
            var sharedFilesList = sharedFiles.ToList();

            foreach (var file in sharedFilesList)
            {
                var coreTier = StructureTheFile(file);
                AddFileToTheSystem(coreTier, VirtualFileSystemTiers);
            }

            foreach (LowLevelFileType type in Enum.GetValues(typeof(LowLevelFileType)))
            {
                var category = new VirtualFileSystemCategory(type.ToString(), sharedFilesList.Where(f => f.Type == type).ToList());
                VirtualFileSystemCategories.Add(category);
            }
        }

        private VirtualFileSystemTier StructureTheFile(ApiBlockRecordFile file)
        {
            var directories = file.Folder.Split('/');
            var totalDepth = directories.Length;

            VirtualFileSystemTier coreTier = null;
            VirtualFileSystemTier higherTier = null;
            for (int i = 0; i < totalDepth; i++)
            {
                var tier = new VirtualFileSystemTier(directories[i], i);

                if (coreTier == null)
                {
                    coreTier = tier;
                    higherTier = coreTier;
                }
                else
                {
                    higherTier.VirtualFileSystemTiers.Add(tier);
                    higherTier = tier;

                    if (i == totalDepth - 1)
                    {
                        tier.Files = new List<ApiBlockRecordFile> { file };
                    }
                }
            }

            return coreTier;
        }

        private void AddFileToTheSystem(VirtualFileSystemTier candidateTier, List<VirtualFileSystemTier> sameLevelFileSystemTiers)
        {
            if (sameLevelFileSystemTiers.Count != 0 && sameLevelFileSystemTiers.Any(t => t.Depth != candidateTier.Depth))
            {
                throw new ArgumentException($"{nameof(candidateTier)} and {nameof(sameLevelFileSystemTiers)} are not on the same level");
            }

            var matchingTierThatIsAlreadyInTheFileSystem = sameLevelFileSystemTiers.FirstOrDefault(t => t.Name == candidateTier.Name);
            if (matchingTierThatIsAlreadyInTheFileSystem == null)
            {
                sameLevelFileSystemTiers.Add(candidateTier);
            }
            else
            {
                var candidateTierDescendant = candidateTier.VirtualFileSystemTiers.FirstOrDefault();
                if (candidateTierDescendant == null)
                {
                    matchingTierThatIsAlreadyInTheFileSystem.Files.Add(candidateTier.Files.First());
                    return;
                }

                AddFileToTheSystem(candidateTierDescendant, matchingTierThatIsAlreadyInTheFileSystem.VirtualFileSystemTiers);
            }
        }
    }

    public class VirtualFileSystemCategory
    {
        public VirtualFileSystemCategory(string category, List<ApiBlockRecordFile> categoryFiles)
        {
            Name = category;
            Files = categoryFiles;
        }

        public string Name { get; }

        public List<ApiBlockRecordFile> Files { get; set; }
    }
}