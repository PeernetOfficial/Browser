using Peernet.Browser.Models.Domain.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Peernet.Browser.Application.VirtualFileSystem
{
    public class VirtualFileSystem
    {
        private readonly IFilesToCategoryBinder binder;

        public VirtualFileSystem(IEnumerable<ApiFile> sharedFiles, IFilesToCategoryBinder binder)
        {
            this.binder = binder;

            if (sharedFiles != null)
            {
                CreateFileSystemStructure(sharedFiles);
            }
        }

        public ObservableCollection<VirtualFileSystemCoreCategory> VirtualFileSystemCategories { get; set; } = new();

        public ObservableCollection<VirtualFileSystemCoreTier> VirtualFileSystemTiers { get; set; } = new();

        public void ResetSelection()
        {
            VirtualFileSystemTiers.Foreach(e => e.ResetSelection());
            VirtualFileSystemCategories.Foreach(e => e.ResetSelection());
        }

        private void AddFileToTheSystem(VirtualFileSystemCoreTier candidateCoreTier, List<VirtualFileSystemEntity> sameLevelFileSystemTiers)
        {
            var matchingTierThatIsAlreadyInTheFileSystem = sameLevelFileSystemTiers.FirstOrDefault(t => t.Name == candidateCoreTier.Name) as VirtualFileSystemCoreTier;
            if (matchingTierThatIsAlreadyInTheFileSystem == null)
            {
                sameLevelFileSystemTiers.Add(candidateCoreTier);
            }
            else
            {
                var candidateTierDescendant =
                    candidateCoreTier.VirtualFileSystemEntities.FirstOrDefault(e => e is VirtualFileSystemCoreTier);
                if (candidateTierDescendant == null)
                {
                    matchingTierThatIsAlreadyInTheFileSystem.VirtualFileSystemEntities.Add(candidateCoreTier.VirtualFileSystemEntities.First());
                    return;
                }

                AddFileToTheSystem(candidateTierDescendant as VirtualFileSystemCoreTier, matchingTierThatIsAlreadyInTheFileSystem.VirtualFileSystemEntities);
            }
        }

        // Organize Files into the structured system
        private void CreateFileSystemStructure(IEnumerable<ApiFile> sharedFiles)
        {
            // materialize
            var sharedFilesList = sharedFiles.ToList();
            var rootTier = new VirtualFileSystemCoreTier("Root", VirtualFileSystemEntityType.Directory);

            foreach (var coreTier in sharedFilesList.Select(StructureTheFile))
            {
                AddFileToTheSystem(coreTier, rootTier.VirtualFileSystemEntities);
            }

            VirtualFileSystemTiers.Add(rootTier);

            VirtualFileSystemCategories = new ObservableCollection<VirtualFileSystemCoreCategory>(binder.Bind(sharedFilesList));
        }

        private VirtualFileSystemCoreTier StructureTheFile(ApiFile file)
        {
            var directories = file.Folder.Split('/');
            var totalDepth = directories.Length;

            VirtualFileSystemCoreTier coreTier = null;
            VirtualFileSystemCoreTier higherTier = null;
            for (int i = 0; i < totalDepth; i++)
            {
                var tier = new VirtualFileSystemCoreTier(directories[i], VirtualFileSystemEntityType.Directory);

                if (coreTier == null)
                {
                    coreTier = tier;
                    higherTier = coreTier;
                }
                else
                {
                    higherTier.VirtualFileSystemEntities.Add(tier);
                    higherTier = tier;
                }

                if (i == totalDepth - 1)
                {
                    var entity = new VirtualFileSystemEntity(file);
                    tier.VirtualFileSystemEntities.Add(entity);
                }
            }

            return coreTier;
        }
    }
}