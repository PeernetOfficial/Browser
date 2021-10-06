using FakeItEasy;
using FizzWare.NBuilder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Peernet.Browser.Application.VirtualFileSystem;
using Peernet.Browser.Models.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Peernet.Browser.Tests.Application.VirtualFileSystem
{
    [TestClass]
    public class VirtualFilesSystemTests
    {
        private IFilesToCategoryBinder fakeBinder;

        [TestInitialize]
        public void SetUp()
        {
            fakeBinder = A.Fake<IFilesToCategoryBinder>();
        }

        [DataTestMethod]
        [DataRow(3, "root", "sub")]
        [DataRow(4, "dit", "subdir")]
        public void Should_StructureTiers(int size, string rootName, string subName)
        {
            // Arrange
            IList<ApiBlockRecordFile> files = Builder<ApiBlockRecordFile>.CreateListOfSize(size).All()
                .With(x => x.Folder = $"{rootName}/{subName}").Build();

            // Act
            var system = new Browser.Application.VirtualFileSystem.VirtualFileSystem(files, fakeBinder);

            //Assert

            // There should be only one tier
            Assert.AreEqual(1, system.VirtualFileSystemTiers.Count);
            var tier = system.VirtualFileSystemTiers.First();
            Assert.AreEqual(rootName, tier.Name);

            // There should be only one subtier
            Assert.AreEqual(1, tier.VirtualFileSystemTiers.Count);
            var subTier = tier.VirtualFileSystemTiers.First();
            Assert.AreEqual(subName, subTier.Name);
        }

        [TestMethod]
        public void Should_Binder_CategorizeFiles()
        {
            // Arrange
            var files = new List<ApiBlockRecordFile>();

            var expectedCategories = new List<VirtualFileSystemCategory>
            {
                new("cat", VirtualFileSystemEntityType.Audio, files)
            };

            A.CallTo(() => fakeBinder.Bind(A<List<ApiBlockRecordFile>>.Ignored)).Returns(expectedCategories);

            // Act
            var system = new Browser.Application.VirtualFileSystem.VirtualFileSystem(files, fakeBinder);

            // Assert
            var categories = system.VirtualFileSystemCategories;
            Assert.IsNotNull(categories);
            CollectionAssert.AreEqual(expectedCategories, categories);
            Assert.AreSame(expectedCategories, categories);
        }
    }
}