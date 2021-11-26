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

        public VirtualFileSystem(IEnumerable<ApiFile> sharedFiles, IFilesToCategoryBinder binder, bool isCurrentSelection = true)
        {
            this.binder = binder;

            CreateRootCoreTier(sharedFiles, isCurrentSelection);
        }

        public ObservableCollection<VirtualFileSystemCoreCategory> VirtualFileSystemCategories { get; set; } = new();

        public ObservableCollection<VirtualFileSystemCoreTier> VirtualFileSystemTiers { get; set; } = new();

        public void ResetSelection()
        {
            VirtualFileSystemTiers.Foreach(e => e.ResetSelection());
            VirtualFileSystemCategories.Foreach(e => e.ResetSelection());
        }

        public VirtualFileSystemCoreEntity GetCurrentlySelected()
        {
            var selected = VirtualFileSystemTiers.FirstOrDefault(t => t.IsSelected) as VirtualFileSystemCoreEntity ??
                           VirtualFileSystemCategories.FirstOrDefault(c => c.IsSelected);

            return selected;
        }

        private void AddFileToTheSystem(VirtualFileSystemEntity candidateEntity, List<VirtualFileSystemEntity> sameLevelFileSystemTiers)
        {
            var candidateCoreTier = candidateEntity as VirtualFileSystemCoreTier;
            if (candidateCoreTier is null)
            {
                sameLevelFileSystemTiers.Add(candidateEntity);
                return;
            }
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

        private void CreateRootCoreTier(IEnumerable<ApiFile> sharedFiles, bool isCurrentSelection)
        {
            // materialize
            var sharedFilesList = sharedFiles.ToList();
            var rootTier = new VirtualFileSystemCoreTier("Root", VirtualFileSystemEntityType.Directory)
            {
                IsSelected = isCurrentSelection
            };

            foreach (var coreTier in sharedFilesList.Select(StructureTheFile))
            {
                AddFileToTheSystem(coreTier, rootTier.VirtualFileSystemEntities);
            }

            VirtualFileSystemTiers.Add(rootTier);

            VirtualFileSystemCategories = new ObservableCollection<VirtualFileSystemCoreCategory>(binder.Bind(sharedFilesList));
        }

        private VirtualFileSystemEntity StructureTheFile(ApiFile file)
        {
            if (file.Folder.IsNullOrEmpty())
            {
                return new VirtualFileSystemEntity(file);
            }

            var directories = file.Folder.Split('/', '\\').ToList();
            directories.RemoveAll(string.IsNullOrEmpty);
            var totalDepth = directories.Count;

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