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
            var s1 = new Mock<ISettingsManager>();

            //Act
            var o = new ApiClient(s1.Object);

            //Assert
            Assert.IsNotNull(o);
        }
    }
}