using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Clients;

namespace Peernet.Browser.Tests.Infrastructure
{
    [TestClass]
    public class ApiClientTest
    {
        [TestMethod]
        public void CtorTest()
        {
            //Prepare
            var fakeSettingsManager = new Mock<ISettingsManager>();
            fakeSettingsManager.Setup(s => s.ApiUrl).Returns("http://localhost:50000/");

            //Act
            var o = new ApiClient(fakeSettingsManager.Object);

            //Assert
            Assert.IsNotNull(o);
        }
    }
}