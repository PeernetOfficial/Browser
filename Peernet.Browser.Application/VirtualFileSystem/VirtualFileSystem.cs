using Peernet.Browser.Application.Extensions;
using Peernet.Browser.Models.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Peernet.Browser.Models.Domain.Common;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystem
    {
        private readonly IFilesToCategoryBinder binder;

        public VirtualFileSystem(IEnumerable<ApiBlockRecordFile> sharedFiles, IFilesToCategoryBinder binder)
        {
            this.binder = binder;

            if (sharedFiles != null)
            {
                CreateFileSystemStructure(sharedFiles);
            }
        }

        public ObservableCollection<VirtualFileSystemCategory> VirtualFileSystemCategories { get; set; } = new();

        public ObservableCollection<VirtualFileSystemTier> VirtualFileSystemTiers { get; set; } = new();

        public void ResetSelection()
        {
            VirtualFileSystemTiers.Foreach(e => e.ResetSelection());
            VirtualFileSystemCategories.Foreach(e => e.ResetSelection());
        }

        private void AddFileToTheSystem(VirtualFileSystemTier candidateTier, ObservableCollection<VirtualFileSystemTier> sameLevelFileSystemTiers)
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

        // Organize Files into the structured system
        private void CreateFileSystemStructure(IEnumerable<ApiBlockRecordFile> sharedFiles)
        {
            // materialize
            var sharedFilesList = sharedFiles.ToList();

            foreach (var coreTier in sharedFilesList.Select(StructureTheFile))
            {
                AddFileToTheSystem(coreTier, VirtualFileSystemTiers);
            }

            VirtualFileSystemCategories = new ObservableCollection<VirtualFileSystemCategory>(binder.Bind(sharedFilesList));
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
                }

                if (i == totalDepth - 1)
                {
                    tier.Files.Add(file);
                }
            }

            return coreTier;
        }
    }
}