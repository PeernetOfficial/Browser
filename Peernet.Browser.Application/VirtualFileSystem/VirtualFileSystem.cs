using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Peernet.Browser.Application.Models;

namespace Peernet.Browser.Application.VirtualFileSystem
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

            FilesToCategoryBinder binder = new(sharedFilesList);
            VirtualFileSystemCategories = binder.Bind();
        }

        private VirtualFileSystemTier StructureTheFile(ApiBlockRecordFile file)
        {
            var directories = file.Folder.Split('/');
            var totalDepth = directories.Length;

            VirtualFileSystemTier coreTier = null;
            VirtualFileSystemTier higherTier = null;
            for (int i = 0; i < totalDepth; i++)
            {
                var tier = new VirtualFileSystemTier(directories[i], VirtualFileSystemEntityType.Directory, i);

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
                        tier.Files.Add(file);
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

        private static string GetPluralInvariant(string s)
        {
            const char pluralSuffix = 's';
            var last = s.Last();
            if (last != 's')
            {
                StringBuilder builder = new(s);
                builder.Append(pluralSuffix);
                s = builder.ToString();
            }

            return s;
        }
    }
}