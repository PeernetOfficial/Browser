using System;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Application.Models
{
    public class VirtualFileSystem
    {
        public VirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles)
        {
            VirtualFileSystemTiers = new List<VirtualFileSystemTier>();
            CreateFileSystemStructure(sharedFiles);
        }

        public List<VirtualFileSystemTier> VirtualFileSystemTiers { get; set; }

        // Organize Files into the structured system
        private void CreateFileSystemStructure(IEnumerable<ApiBlockRecordFile> sharedFiles)
        {
            foreach (var file in sharedFiles)
            {
                var coreTier = StructureTheFile(file);
                AddFileToTheSystem(coreTier, VirtualFileSystemTiers);
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
                    higherTier.FileSystemTiers.Add(tier);
                    higherTier = tier;

                    if (i == totalDepth - 1)
                    {
                        tier.Files = new ApiBlockchainAddFiles
                        {
                            Files = new List<ApiBlockRecordFile> { file }
                        };
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
                var candidateTierDescendant = candidateTier.FileSystemTiers.FirstOrDefault();
                if (candidateTierDescendant == null)
                {
                    matchingTierThatIsAlreadyInTheFileSystem.Files.Files.Add(candidateTier.Files.Files.First());
                    return;
                }

                AddFileToTheSystem(candidateTierDescendant, matchingTierThatIsAlreadyInTheFileSystem.FileSystemTiers);
            }
        }
    }
}