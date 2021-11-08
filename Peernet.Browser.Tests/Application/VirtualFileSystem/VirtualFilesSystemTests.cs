using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain.Common;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Tests.Application.VirtualFileSystem
{
    [TestClass]
    public class VirtualFilesSystemTests
    {
        private Mock<IFilesToCategoryBinder> fakeBinder;

        [TestInitialize]
        public void SetUp()
        {
            fakeBinder = new Mock<IFilesToCategoryBinder>();
        }

        [DataTestMethod]
        [DataRow(3, "root", "sub")]
        [DataRow(4, "dit", "subdir")]
        public void Should_StructureTiers(int size, string rootName, string subName)
        {
            // Arrange
            IList<ApiFile> files = Builder<ApiFile>.CreateListOfSize(size).All()
                .With(x => x.Folder = $"{rootName}/{subName}").Build();
            fakeBinder.Setup(s => s.Bind(It.IsAny<IEnumerable<ApiFile>>())).Returns(new List<VirtualFileSystemCoreCategory>());

            // Act
            var system = new Browser.Application.VirtualFileSystem.VirtualFileSystem(files, fakeBinder.Object);

            //Assert

            // There should be only one tier
            Assert.AreEqual(1, system.VirtualFileSystemTiers.Count);
            var tier = system.VirtualFileSystemTiers.First();
            Assert.AreEqual(rootName, tier.Name);

            // There should be only one subtier
            Assert.AreEqual(1, tier.VirtualFileSystemEntities.Count);
            var subTier = tier.VirtualFileSystemEntities.First();
            Assert.AreEqual(subName, subTier.Name);
        }

        [TestMethod]
        public void Should_Binder_CategorizeFiles()
        {
            // Arrange
            var files = new List<ApiFile>();

            var expectedCategories = new List<VirtualFileSystemCoreCategory>
            {
                new("cat", VirtualFileSystemEntityType.Audio, files)
            };

            fakeBinder.Setup(s => s.Bind(It.IsAny<IEnumerable<ApiFile>>())).Returns(expectedCategories);

            // Act
            var system = new Browser.Application.VirtualFileSystem.VirtualFileSystem(files, fakeBinder.Object);

            // Assert
            var categories = system.VirtualFileSystemCategories;
            Assert.IsNotNull(categories);
            CollectionAssert.AreEqual(expectedCategories, categories);
        }
    }
}