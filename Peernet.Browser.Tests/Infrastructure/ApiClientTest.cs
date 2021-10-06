using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Peernet.Browser.Application.Http;
using Peernet.Browser.Infrastructure;
using Peernet.Browser.Infrastructure.Wrappers;

namespace Peernet.Browser.Tests.Infrastructure
{
    [TestClass]
    public class ApiClientTest
    {
        [TestMethod]
        public void CtorTest()
        {
            //Prepare
            var s1 = new Mock<IHttpClientFactory>();

            //Act
            var o = new ApiWrapper(s1.Object);

            //Assert
            Assert.IsNotNull(o);
        }
    }
}