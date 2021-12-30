using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Peernet.Browser.Application.Managers;
using Peernet.Browser.Infrastructure.Clients;
using Peernet.Browser.Infrastructure.Http;

namespace Peernet.Browser.Tests.Infrastructure
{
    [TestClass]
    public class ApiClientTest
    {
        [TestMethod]
        public void CtorTest()
        {
            //Prepare
            var httpExecutorMock = new Mock<IHttpExecutor>();

            //Act
            var o = new ApiClient(httpExecutorMock.Object);

            //Assert
            Assert.IsNotNull(o);
        }
    }
}